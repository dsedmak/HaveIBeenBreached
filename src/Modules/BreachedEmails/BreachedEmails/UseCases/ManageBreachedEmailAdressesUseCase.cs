using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.CallerContract;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.UseCases
{
    internal class ManageBreachedEmailAdressesUseCase : IManageBreachedEmailAdresses
    {
        private readonly IAddBreachedEmailAddress _addBreachedEmailAddress;
        private readonly ICheckEmailAddress _checkEmailAddress;
        private readonly IRemoveBreachedEmailAddress _removeBreachedEmailAddress;

        public ManageBreachedEmailAdressesUseCase(
            IAddBreachedEmailAddress addBreachedEmailAddress,
            IRemoveBreachedEmailAddress removeBreachedEmailAddress,
            ICheckEmailAddress checkEmailAddress)
        {
            _addBreachedEmailAddress = addBreachedEmailAddress;
            _removeBreachedEmailAddress = removeBreachedEmailAddress;
            _checkEmailAddress = checkEmailAddress;
        }

        /// <inheritdoc />
        public ValueTask<Result> AddBreachedEmailAddress(string emailAddress)
        {
            return _addBreachedEmailAddress.AddBreachedEmailAddress(emailAddress);
        }

        /// <inheritdoc />
        public ValueTask<Result> RemoveBreachedEmailAddress(string emailAddress)
        {
            return _removeBreachedEmailAddress.RemoveBreachedEmailAddress(emailAddress);
        }

        /// <inheritdoc />
        public ValueTask<Result<bool>> CheckEmailAddress(string emailAddress)
        {
            return _checkEmailAddress.CheckEmailAddress(emailAddress);
        }
    }
}