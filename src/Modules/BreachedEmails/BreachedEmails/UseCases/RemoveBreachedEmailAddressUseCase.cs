using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.CallerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.UseCases
{
    /// <inheritdoc />
    internal class RemoveBreachedEmailAddressUseCase : IRemoveBreachedEmailAddress
    {
        private readonly IEmailAddressCollection _emailAddressCollection;

        public RemoveBreachedEmailAddressUseCase(IEmailAddressCollection emailAddressCollection)
        {
            _emailAddressCollection = emailAddressCollection;
        }

        /// <inheritdoc />
        public async ValueTask<Result> RemoveBreachedEmailAddress(string emailAddress)
        {
            // TODO: implement something like TransactionScope to group commits for the entire operation (TransactionScope has issues)
            var createEmailAddressResult = EmailAddress.Create(emailAddress);
            if (createEmailAddressResult.IsSuccess)
            {
                var result = await _emailAddressCollection.Remove(createEmailAddressResult.Value!);
                result.Reasons!.RemoveAll(reason => reason is EmailAddressNotFoundError);
                return result;
            }

            return createEmailAddressResult;
        }
    }
}