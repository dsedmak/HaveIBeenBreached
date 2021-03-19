using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using KTrie;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.InMemoryEmailAddressCollectionAdapter
{
    /// <inheritdoc />
    internal class InMemoryEmailAddressCollection : IEmailAddressCollection
    {
        private readonly IEmailAddressCollection _authority;
        private readonly StringTrieSet _emailAdresses = new(); // TODO: this needs to be thread-safe

        public InMemoryEmailAddressCollection(IEmailAddressCollection authority)
        {
            _authority = authority;
        }

        /// <inheritdoc />
        public ValueTask<bool> Contains(EmailAddress emailAddress)
        {
            return new(_emailAdresses.Contains(ReverseEmailAddress(emailAddress.Value)));
        }

        /// <inheritdoc />
        public async ValueTask<Result> Add(EmailAddress emailAddress)
        {
            var reversedEmailAddress = ReverseEmailAddress(emailAddress.Value);

            if (_emailAdresses.Contains(reversedEmailAddress))
            {
                return Result.Fail(new EmailAddressAlreadyExistsError())!;
            }

            var authorityResult = await _authority.Add(emailAddress);
            if (authorityResult.IsSuccess)
            {
                _emailAdresses.Add(reversedEmailAddress);
            }

            return authorityResult;
        }

        /// <inheritdoc />
        public async ValueTask<Result> Remove(EmailAddress emailAddress)
        {
            var reversedEmailAddress = ReverseEmailAddress(emailAddress.Value);
            if (!_emailAdresses.Contains(reversedEmailAddress))
            {
                return Result.Fail(new EmailAddressNotFoundError())!;
            }

            var authorityResult = await _authority.Remove(emailAddress);
            if (authorityResult.IsFailed)
            {
                return authorityResult;
            }

            _emailAdresses.Remove(reversedEmailAddress);
            return authorityResult;
        }

        /// <inheritdoc />
        public async IAsyncEnumerator<EmailAddress> GetAsyncEnumerator(CancellationToken cancellationToken = new())
        {
            await foreach (var emailAdress in _authority)
            {
                _emailAdresses.Add(ReverseEmailAddress(emailAdress.Value));
                yield return emailAdress;
            }
        }

        // Reverses the email to reduce trie branching
        private static string ReverseEmailAddress(string value)
        {
            var charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}