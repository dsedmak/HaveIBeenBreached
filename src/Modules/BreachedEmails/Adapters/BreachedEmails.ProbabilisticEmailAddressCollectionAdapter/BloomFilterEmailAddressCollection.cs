using Bloomy.Lib.Filter;
using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.ProbabilisticEmailAddressCollectionAdapter
{
    /// <inheritdoc />
    internal class BloomFilterEmailAddressCollection : IEmailAddressCollection
    {
        private readonly IEmailAddressCollection? _authority;
        private readonly BasicFilter _emailAdresses; // TODO: this needs to be thread-safe
        private readonly ProbabilisticEmailAddressCollectionOptions _options;

        public BloomFilterEmailAddressCollection(
            IOptions<ProbabilisticEmailAddressCollectionOptions> options,
            IEmailAddressCollection? authority = default)
        {
            _authority = authority;
            _options = options.Value;
            _emailAdresses = new BasicFilter(_options.MemoryAvailableInBytes * 8, HashFunc.SHA256, 8);
        }

        /// <inheritdoc />
        public ValueTask<bool> Contains(EmailAddress emailAddress)
        {
            var filterResult = _emailAdresses.Check(emailAddress.Value)!;
            if (_authority is null || _options.CollectionBehaviour == CollectionBehaviour.Probabilistic)
            {
                // bug in the library: filterResult.Probability should be 1 - itself when BloomPresence.MightBeInserted
                var probabilityThatResultIsCorrect = filterResult.Presence == BloomPresence.NotInserted
                    ? filterResult.Probability
                    : 1 - filterResult.Probability;

                if (probabilityThatResultIsCorrect > _options.ProbabilisticThreshold || _authority is null)
                {
                    return new ValueTask<bool>(filterResult.Presence == BloomPresence.MightBeInserted);
                }
            }

            if (filterResult.Presence == BloomPresence.MightBeInserted)
            {
                return _authority.Contains(emailAddress);
            }

            return new ValueTask<bool>(false);
        }

        /// <inheritdoc />
        public async ValueTask<Result> Add(EmailAddress emailAddress)
        {
            if (_emailAdresses.Check(emailAddress.Value)!.Presence == BloomPresence.MightBeInserted)
            {
                if (_authority is null || await _authority.Contains(emailAddress))
                {
                    return Result.Fail(new EmailAddressAlreadyExistsError())!;
                }
            }

            if (_authority is not null)
            {
                var authorityResult = await _authority.Add(emailAddress);
                if (authorityResult.IsFailed)
                {
                    return authorityResult;
                }
            }

            _emailAdresses.Insert(emailAddress.Value);

            return Result.Ok()!;
        }

        /// <inheritdoc />
        public async ValueTask<Result> Remove(EmailAddress emailAddress)
        {
            if (_emailAdresses.Check(emailAddress.Value)!.Presence == BloomPresence.NotInserted)
            {
                return Result.Fail(new EmailAddressNotFoundError());
            }

            if (_authority is not null)
            {
                var authorityResult = await _authority.Remove(emailAddress);
                if (authorityResult.IsFailed)
                {
                    return authorityResult;
                }
            }

            // bloom filter does not supporte removal
            // the result is simply a higher false positive rate
            return Result.Ok()!;
        }

        /// <inheritdoc />
        public async IAsyncEnumerator<EmailAddress> GetAsyncEnumerator(CancellationToken cancellationToken = new())
        {
            if (_authority is null)
            {
                yield break;
            }

            await foreach (var emailAddress in _authority)
            {
                _emailAdresses.Insert(emailAddress.Value);
                yield return emailAddress;
            }
        }
    }
}