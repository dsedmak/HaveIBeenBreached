using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using GenePlanet.HaveIBeenBreached.BreachedEmails.UseCases;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.Tests.BreachedEmails.UseCases
{
    [TestFixture]
    public class RemoveBreachedEmailAddressUseCaseTests
    {
        [Test]
        public async Task ReturnsOkIfEmailAddressDoesNotExist()
        {
            var emailAddressCollectionMock = new Mock<IEmailAddressCollection>();
            emailAddressCollectionMock.Setup(mock => mock.Remove(It.IsAny<EmailAddress>())).Returns(() =>
                ValueTask.FromResult(Result.Fail(new EmailAddressNotFoundError())));
            var sut = new RemoveBreachedEmailAddressUseCase(emailAddressCollectionMock.Object!);
            
            var result = await sut.RemoveBreachedEmailAddress("valid@email.cm");
            
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public async Task ReturnsOkIfEmailAddressExists()
        {
            var emailAddressCollectionMock = new Mock<IEmailAddressCollection>();
            emailAddressCollectionMock.Setup(mock => mock.Remove(It.IsAny<EmailAddress>())).Returns(() =>
                ValueTask.FromResult(Result.Ok()));
            var sut = new RemoveBreachedEmailAddressUseCase(emailAddressCollectionMock.Object!);
            
            var result = await sut.RemoveBreachedEmailAddress("valid@email.cm");
            
            Assert.That(result.IsSuccess, Is.True);
        }
    }
}