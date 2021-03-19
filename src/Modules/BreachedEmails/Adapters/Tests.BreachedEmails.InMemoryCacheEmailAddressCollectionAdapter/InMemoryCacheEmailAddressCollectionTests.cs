using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.InMemoryCacheEmailAddressCollectionAdapter;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.Tests.BreachedEmails.InMemoryCacheEmailAddressCollectionAdapter
{
    [TestFixture]
    public class InMemoryCacheEmailAddressCollectionTests
    {
        private readonly IOptions<InMemoryCacheEmailAddressCollectionOptions> _options;

        public InMemoryCacheEmailAddressCollectionTests()
        {
            _options = new OptionsWrapper<InMemoryCacheEmailAddressCollectionOptions>(new()
            {
                MemoryAvailableInBytes = 8192
            });
        }

        private static IEmailAddressCollection GenerateMockAuthority(bool containsResult, Result addResult, Result removeResult)
        {
            var mock = new Mock<IEmailAddressCollection>();
            mock.Setup(m => m.Contains(It.IsAny<EmailAddress>())).Returns(() => ValueTask.FromResult(containsResult));
            mock.Setup(m => m.Add(It.IsAny<EmailAddress>())).Returns(() => ValueTask.FromResult(addResult));
            mock.Setup(m => m.Remove(It.IsAny<EmailAddress>())).Returns(() => ValueTask.FromResult(removeResult));
            return mock.Object!;
        }

        [Test]
        public async Task ReturnsOkFalseIfEmailAddressDoesNotExist()
        {
            var authority = GenerateMockAuthority(containsResult: false, Result.Ok()!, Result.Ok()!);
            var sut = new InMemoryCacheEmailAddressCollection(_options, authority);
            
            var contains = await sut.Contains(EmailAddress.Create("valid@email.cm").Value);
            
            Assert.That(contains, Is.False);
        }
        
        [Test]
        public async Task ReturnsOkTrueIfEmailAddressExists()
        {
            var authority = GenerateMockAuthority(containsResult: true, addResult: Result.Ok()!, Result.Ok()!);
            var sut = new InMemoryCacheEmailAddressCollection(_options, authority);
            var emailAddress = EmailAddress.Create("valid@email.cm").Value;

            var addResult = await sut.Add(emailAddress);
            Assert.That(addResult.IsSuccess, Is.True);
            
            var containsResult = await sut.Contains(emailAddress);
            Assert.That(containsResult, Is.True);
        }
        
        [Test]
        public async Task ReturnsEmailAddressAlreadyExistsErrorIfEmailAddressExists()
        {
            var authority = GenerateMockAuthority(containsResult: true, addResult: Result.Ok()!, Result.Ok()!);
            var sut = new InMemoryCacheEmailAddressCollection(_options, authority);
            var emailAddress = EmailAddress.Create("valid@email.cm").Value;

            var addResult = await sut.Add(emailAddress);
            Assert.That(addResult.IsSuccess, Is.True);
            
            var containsResult = await sut.Add(emailAddress);
            Assert.That(containsResult.Reasons, Has.Exactly(1).InstanceOf<EmailAddressAlreadyExistsError>());
        }
        
        [Test]
        public async Task ReturnsEmailAddressNotFoundErrorIfEmailAddressDoesNotExists()
        {
            var authority = GenerateMockAuthority(containsResult: true, Result.Ok()!, removeResult: Result.Fail(new EmailAddressNotFoundError())!);
            var sut = new InMemoryCacheEmailAddressCollection(_options, authority);
            
            var removeResult = await sut.Remove(EmailAddress.Create("valid@email.cm").Value);
            
            Assert.That(removeResult.Reasons, Has.Exactly(1).InstanceOf<EmailAddressNotFoundError>());
        }
        
        [Test]
        public async Task ReturnsOkIfEmailAddressExists()
        {
            var authority = GenerateMockAuthority(containsResult: true, addResult: Result.Ok()!, removeResult: Result.Ok()!);
            var sut = new InMemoryCacheEmailAddressCollection(_options, authority);
            var emailAddress = EmailAddress.Create("valid@email.cm").Value;

            var addResult = await sut.Add(emailAddress);
            Assert.That(addResult.IsSuccess, Is.True);
            
            var removeResult = await sut.Remove(emailAddress);
            Assert.That(removeResult.IsSuccess, Is.True);
        }
    }
}