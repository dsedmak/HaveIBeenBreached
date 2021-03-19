/*using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.CallerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using GenePlanet.HaveIBeenBreached.BreachedEmails.UseCases;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.Tests.BreachedEmails.UseCases
{
    [TestFixture]
    public class ManageBreachedEmailAdressesUseCaseTests
    {
        public ManageBreachedEmailAdressesUseCaseTests()
        {
            var addBreachedEmailAddressUseCaseMock = new Mock<IAddBreachedEmailAddress>();
            var removeBreachedEmailAddressUseCaseMock = new Mock<IRemoveBreachedEmailAddress>();
            var checkEmailAddressUseCaseMock = new Mock<ICheckEmailAddress>();
        }
        
        public async Task CallsAddBreachedEmailAdressUseCaseOnAdd()
        {
            var sut = new AddBreachedEmailAddressUseCase(emailAddressCollectionMock.Object!);
            
            var result = await sut.AddBreachedEmailAddress("valid@email.cm");
            
            Assert.That(result.Reasons, Has.Exactly(1).InstanceOf<EmailAddressAlreadyExistsError>());
        }

        public async Task CallsRemoveBreachedEmailAdressUseCaseOnRemove()
        {
            var emailAddressCollectionMock = new Mock<IEmailAddressCollection>();
            emailAddressCollectionMock.Setup(mock => mock.Add(It.IsAny<EmailAddress>())).Returns(() =>
                ValueTask.FromResult(Result.Ok()));
            var sut = new AddBreachedEmailAddressUseCase(emailAddressCollectionMock.Object!);
            
            var result = await sut.AddBreachedEmailAddress("valid@email.cm");
            
            Assert.That(result.IsSuccess, Is.True);
        }

        public async Task CallsCheckEmailAdressUseCaseOnCheck()
        {
            var emailAddressCollectionMock = new Mock<IEmailAddressCollection>();
            emailAddressCollectionMock.Setup(mock => mock.Add(It.IsAny<EmailAddress>())).Returns(() =>
                ValueTask.FromResult(Result.Ok()));
            var sut = new AddBreachedEmailAddressUseCase(emailAddressCollectionMock.Object!);
            
            var result = await sut.AddBreachedEmailAddress("valid@email.cm");
            
            Assert.That(result.IsSuccess, Is.True);
        }
    }
}*/