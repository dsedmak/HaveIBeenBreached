using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.InMemoryEmailAddressCollectionAdapter;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.Tests.BreachedEmails.InMemoryEmailAddressCollectionAdapter
{
    [TestFixture]
    public class InMemoryEmailAddressCollectionTests
    {
        private readonly IEmailAddressCollection _authority;

        public InMemoryEmailAddressCollectionTests()
        {
            var mock = new Mock<IEmailAddressCollection>();
            mock.Setup(m => m.Contains(It.IsAny<EmailAddress>())).Returns(() => ValueTask.FromResult(true));
            mock.Setup(m => m.Add(It.IsAny<EmailAddress>())).Returns(() => ValueTask.FromResult(Result.Ok()));
            mock.Setup(m => m.Remove(It.IsAny<EmailAddress>())).Returns(() => ValueTask.FromResult(Result.Ok()));
            _authority = mock.Object!;
        }
        
        [Test]
        public async Task ReturnsOkFalseIfEmailAddressDoesNotExist()
        {
            var sut = new InMemoryEmailAddressCollection(_authority);
            
            var contains = await sut.Contains(EmailAddress.Create("valid@email.cm").Value);
            
            Assert.That(contains, Is.False);
        }
        
        [Test]
        public async Task ReturnsOkTrueIfEmailAddressExists()
        {
            var sut = new InMemoryEmailAddressCollection(_authority);
            var emailAddress = EmailAddress.Create("valid@email.cm").Value;

            var addResult = await sut.Add(emailAddress);
            Assert.That(addResult.IsSuccess, Is.True);
            
            var containsResult = await sut.Contains(emailAddress);
            Assert.That(containsResult, Is.True);
        }
        
        [Test]
        public async Task ReturnsEmailAddressAlreadyExistsErrorIfEmailAddressExists()
        {
            var sut = new InMemoryEmailAddressCollection(_authority);
            var emailAddress = EmailAddress.Create("valid@email.cm").Value;

            var addResult = await sut.Add(emailAddress);
            Assert.That(addResult.IsSuccess, Is.True);
            
            var containsResult = await sut.Add(emailAddress);
            Assert.That(containsResult.Reasons, Has.Exactly(1).InstanceOf<EmailAddressAlreadyExistsError>());
        }
        
        [Test]
        public async Task ReturnsEmailAddressNotFoundErrorIfEmailAddressDoesNotExists()
        {
            var sut = new InMemoryEmailAddressCollection(_authority);
            
            var removeResult = await sut.Remove(EmailAddress.Create("valid@email.cm").Value);
            
            Assert.That(removeResult.Reasons, Has.Exactly(1).InstanceOf<EmailAddressNotFoundError>());
        }
        
        [Test]
        public async Task ReturnsOkIfEmailAddressExists()
        {
            var sut = new InMemoryEmailAddressCollection(_authority);
            var emailAddress = EmailAddress.Create("valid@email.cm").Value;

            var addResult = await sut.Add(emailAddress);
            Assert.That(addResult.IsSuccess, Is.True);
            
            var removeResult = await sut.Remove(emailAddress);
            Assert.That(removeResult.IsSuccess, Is.True);
        }
    }
}