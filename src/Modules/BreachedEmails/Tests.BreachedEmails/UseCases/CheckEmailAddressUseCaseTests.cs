using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using GenePlanet.HaveIBeenBreached.BreachedEmails.UseCases;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.Tests.BreachedEmails.UseCases
{
    [TestFixture]
    public class CheckEmailAddressUseCaseTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        [TestCase("")]
        [TestCase("@@")]
        [TestCase("a@@b")]
        [TestCase("@")]
        [TestCase("ndoiasndonsaiodoaisnsaondasnoindoisanaonoadnoasnodnanaosdnasoand@mdaosopdpoasmpdmpasmpamdpsaodmassapdmapsmamsdpmaposadmpaosmdmdapsandsaondasdoisdannasdonaondndbasuoibudbnasbduiabdsnibdiubduibasidibasbdasisabdauibduiabauissabdsaiubdaisubdibasidbuibasuidbuibaisubduiabiuabidubaiiudbiabsiubduiasbuidbasi.dsa.dsadsa")]
        public async Task ReturnsInvalidEmailErrorIfInputIsNotAValidEmailAddress(string input)
        {
            var emailAddressCollectionMock = new Mock<IEmailAddressCollection>();
            var sut = new CheckEmailAddressUseCase(emailAddressCollectionMock.Object!);
            
            var result = await sut.CheckEmailAddress(input);
            
            Assert.That(result.Reasons, Has.Exactly(1).InstanceOf<InvalidEmailError>());
        }

        [Test]
        public async Task ReturnsOkFalseIfEmailAddressDoesNotExist()
        {
            var emailAddressCollectionMock = new Mock<IEmailAddressCollection>();
            emailAddressCollectionMock.Setup(mock => mock.Contains(It.IsAny<EmailAddress>())).Returns(() =>
                ValueTask.FromResult(false));
            var sut = new CheckEmailAddressUseCase(emailAddressCollectionMock.Object!);
            
            var result = await sut.CheckEmailAddress("valid@email.cm");
            
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.False);
        }

        [Test]
        public async Task ReturnsOkTrueIfEmailAddressExists()
        {
            var emailAddressCollectionMock = new Mock<IEmailAddressCollection>();
            emailAddressCollectionMock.Setup(mock => mock.Contains(It.IsAny<EmailAddress>())).Returns(() =>
                ValueTask.FromResult(true));
            var sut = new CheckEmailAddressUseCase(emailAddressCollectionMock.Object!);
            
            var result = await sut.CheckEmailAddress("valid@email.cm");
            
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.True);
        }
    }
}