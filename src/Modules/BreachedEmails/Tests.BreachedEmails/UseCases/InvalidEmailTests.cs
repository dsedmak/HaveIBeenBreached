using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using GenePlanet.HaveIBeenBreached.BreachedEmails.UseCases;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.Tests.BreachedEmails.UseCases
{
    [TestFixture]
    public class InvalidEmailTests
    {
        [DatapointSource]
        private static readonly string?[] TestCases = {
            "ndoiasndonsaiodoaisnsaondasnoindoisanaonoadnoasnodnanaosdnasoand@mdaosopdpoasmpdmpasmpamdpsaodmassapdmapsmamsdpmaposadmpaosmdmdapsandsaondasdoisdannasdonaondndbasuoibudbnasbduiabdsnibdiubduibasidibasbdasisabdauibduiabauissabdsaiubdaisubdibasidbuibasuidbuibaisubduiabiuabidubaiiudbiabsiubduiasbuidbasi.dsa.dsadsa",
            null, string.Empty, "    ", "@@", "a@@b", "@"
        };
        
        [Theory]
        public async Task AddReturnsInvalidEmailErrorIfInputIsNotAValidEmailAddress(string input)
        {
            var emailAddressCollectionMock = new Mock<IEmailAddressCollection>();
            var sut = new AddBreachedEmailAddressUseCase(emailAddressCollectionMock.Object!);
            
            var result = await sut.AddBreachedEmailAddress(input);
            
            Assert.That(result.Reasons, Has.Exactly(1).InstanceOf<InvalidEmailError>());
        }
        
        [Theory]
        public async Task CheckReturnsInvalidEmailErrorIfInputIsNotAValidEmailAddress(string input)
        {
            var emailAddressCollectionMock = new Mock<IEmailAddressCollection>();
            var sut = new CheckEmailAddressUseCase(emailAddressCollectionMock.Object!);
            
            var result = await sut.CheckEmailAddress(input);
            
            Assert.That(result.Reasons, Has.Exactly(1).InstanceOf<InvalidEmailError>());
        }
        
        [Theory]
        public async Task RemoveReturnsInvalidEmailErrorIfInputIsNotAValidEmailAddress(string input)
        {
            var emailAddressCollectionMock = new Mock<IEmailAddressCollection>();
            var sut = new CheckEmailAddressUseCase(emailAddressCollectionMock.Object!);
            
            var result = await sut.CheckEmailAddress(input);
            
            Assert.That(result.Reasons, Has.Exactly(1).InstanceOf<InvalidEmailError>());
        }
    }
}