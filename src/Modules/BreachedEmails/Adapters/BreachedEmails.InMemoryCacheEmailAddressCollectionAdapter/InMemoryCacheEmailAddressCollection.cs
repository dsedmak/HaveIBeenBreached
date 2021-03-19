using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.InMemoryCacheEmailAddressCollectionAdapter
{
    /// <inheritdoc />
    internal class InMemoryCacheEmailAddressCollection : IEmailAddressCollection, IDisposable
    {
        private readonly IEmailAddressCollection _authority;
        private readonly MemoryCache _cache;

        public InMemoryCacheEmailAddressCollection(
            IOptions<InMemoryCacheEmailAddressCollectionOptions> options,
            IEmailAddressCollection authority)
        {
            _authority = authority;
            _cache = new MemoryCache(new MemoryCacheOptions {SizeLimit = options.Value!.MemoryAvailableInBytes});
        }

        /// <inheritdoc />
        public async ValueTask<bool> Contains(EmailAddress emailAddress)
        {
            if (!_cache.TryGetValue(emailAddress.Value, out bool isPresent))
            {
                var authorityContains = await _authority.Contains(emailAddress);
                using var cacheEntry = _cache.CreateEntry(emailAddress.Value);
                cacheEntry!.Size = CalculateEmailAddressSize(emailAddress);
                cacheEntry!.Value = authorityContains;
                isPresent = authorityContains;
            }

            return isPresent;
        }

        /// <inheritdoc />
        public async ValueTask<Result> Add(EmailAddress emailAddress)
        {
            if (_cache.TryGetValue(emailAddress.Value, out bool isPresent) && isPresent)
            {
                return Result.Fail(new EmailAddressAlreadyExistsError())!;
            }

            var authorityResult = await _authority.Add(emailAddress);
            if (authorityResult.IsSuccess || authorityResult.HasError<EmailAddressAlreadyExistsError>())
            {
                _cache.Set(emailAddress.Value, true,
                    new MemoryCacheEntryOptions().SetSize(CalculateEmailAddressSize(emailAddress))!);
            }

            return authorityResult;
        }

        /// <inheritdoc />
        public async ValueTask<Result> Remove(EmailAddress emailAddress)
        {
            if (_cache.TryGetValue(emailAddress.Value, out bool isPresent) && !isPresent)
            {
                return Result.Fail(new EmailAddressNotFoundError())!;
            }

            var authorityResult = await _authority.Remove(emailAddress);
            if (authorityResult.IsSuccess || authorityResult.HasError<EmailAddressNotFoundError>())
            {
                _cache.Remove(emailAddress.Value);
            }

            return authorityResult;
        }

        /// <inheritdoc />
        public IAsyncEnumerator<EmailAddress> GetAsyncEnumerator(CancellationToken cancellationToken = new())
        {
            return _authority.GetAsyncEnumerator(cancellationToken);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _cache.Dispose();
        }

        private static int CalculateEmailAddressSize(EmailAddress emailAddress)
        {
            return Encoding.UTF8.GetByteCount(emailAddress.Value);
        }
    }
}