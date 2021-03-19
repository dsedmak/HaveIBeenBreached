using GenePlanet.HaveIBeenBreached.BreachedEmails.EfCoreEmailAddressCollectionAdapter;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.Tests.BreachedEmails.EfCoreEmailAddressCollectionAdapter
{
    [TestFixture]
    public class EfCoreEmailAddressCollectionTests
    {
        [Test]
        public async Task ReturnsOkFalseIfEmailAddressDoesNotExist()
        {
            var dbContextFactory = await CreateDbContextFactory();
            var sut = new EfCoreEmailAddressCollection(dbContextFactory);
            
            var contains = await sut.Contains(EmailAddress.Create("valid@email.cm").Value);
            
            Assert.That(contains, Is.False);
        }
        
        [Test]
        public async Task ReturnsOkTrueIfEmailAddressExists()
        {
            var dbContextFactory = await CreateDbContextFactory();
            var sut = new EfCoreEmailAddressCollection(dbContextFactory);
            var emailAddress = EmailAddress.Create("valid@email.cm").Value;

            var addResult = await sut.Add(emailAddress);
            Assert.That(addResult.IsSuccess, Is.True);
            
            var containsResult = await sut.Contains(emailAddress);
            Assert.That(containsResult, Is.True);
        }
        
        [Test]
        public async Task ReturnsEmailAddressAlreadyExistsErrorIfEmailAddressExists()
        {
            var dbContextFactory = await CreateDbContextFactory();
            var sut = new EfCoreEmailAddressCollection(dbContextFactory);
            var emailAddress = EmailAddress.Create("valid@email.cm").Value;

            var addResult = await sut.Add(emailAddress);
            Assert.That(addResult.IsSuccess, Is.True);
            
            var containsResult = await sut.Add(emailAddress);
            Assert.That(containsResult.Reasons, Has.Exactly(1).InstanceOf<EmailAddressAlreadyExistsError>());
        }
        
        [Test]
        public async Task ReturnsEmailAddressNotFoundErrorIfEmailAddressDoesNotExists()
        {
            var dbContextFactory = await CreateDbContextFactory();
            var sut = new EfCoreEmailAddressCollection(dbContextFactory);
            
            var removeResult = await sut.Remove(EmailAddress.Create("valid@email.cm").Value);
            
            Assert.That(removeResult.Reasons, Has.Exactly(1).InstanceOf<EmailAddressNotFoundError>());
        }
        
        [Test]
        public async Task ReturnsOkIfEmailAddressExists()
        {
            var dbContextFactory = await CreateDbContextFactory();
            var sut = new EfCoreEmailAddressCollection(dbContextFactory);
            var emailAddress = EmailAddress.Create("valid@email.cm").Value;

            var addResult = await sut.Add(emailAddress);
            Assert.That(addResult.IsSuccess, Is.True);
            
            var removeResult = await sut.Remove(emailAddress);
            Assert.That(removeResult.IsSuccess, Is.True);
        }

        private async Task<IDbContextFactory<BreachedEmailsDbContext>> CreateDbContextFactory([CallerMemberName] string databaseName = default!)
        {
            var serviceProvider = new ServiceCollection().AddDbContextFactory<BreachedEmailsDbContext>(options =>
                options.UseSqlite($"DataSource={databaseName}.db"))!.BuildServiceProvider()!;
            var dbContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<BreachedEmailsDbContext>>();
            await using var dbContext = dbContextFactory.CreateDbContext();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
            return dbContextFactory;
        }
    }
}