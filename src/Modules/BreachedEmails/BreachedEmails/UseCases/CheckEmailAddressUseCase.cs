using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.CallerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.UseCases
{
    /// <inheritdoc />
    internal class CheckEmailAddressUseCase : ICheckEmailAddress
    {
        private readonly IEmailAddressCollection _emailAddressCollection;

        public CheckEmailAddressUseCase(IEmailAddressCollection emailAddressCollection)
        {
            _emailAddressCollection = emailAddressCollection;
        }

        /// <inheritdoc />
        public async ValueTask<Result<bool>> CheckEmailAddress(string emailAddress)
        {
            // TODO: implement something like TransactionScope to group commits for the entire operation (TransactionScope has issues)
            var createEmailAddressResult = EmailAddress.Create(emailAddress);
            if (createEmailAddressResult.IsSuccess)
            {
                var emailAddressIsBreached = await _emailAddressCollection.Contains(createEmailAddressResult.Value!);
                return Result.Ok(emailAddressIsBreached)!;
            }

            return createEmailAddressResult.ToResult();
        }
    }
}