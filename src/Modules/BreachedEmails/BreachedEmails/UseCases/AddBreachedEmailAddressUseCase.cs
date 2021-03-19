using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.CallerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.UseCases
{
    /// <inheritdoc />
    internal class AddBreachedEmailAddressUseCase : IAddBreachedEmailAddress
    {
        private readonly IEmailAddressCollection _emailAddressCollection;

        public AddBreachedEmailAddressUseCase(IEmailAddressCollection emailAddressCollection)
        {
            _emailAddressCollection = emailAddressCollection;
        }

        /// <inheritdoc />
        public ValueTask<Result> AddBreachedEmailAddress(string emailAddress)
        {
            // TODO: implement something like TransactionScope to group commits for the entire operation (TransactionScope has issues)
            var createEmailAddressResult = EmailAddress.Create(emailAddress);
            if (createEmailAddressResult.IsSuccess)
            {
                return _emailAddressCollection.Add(createEmailAddressResult.Value!);
            }

            return new ValueTask<Result>(createEmailAddressResult);
        }
    }
}