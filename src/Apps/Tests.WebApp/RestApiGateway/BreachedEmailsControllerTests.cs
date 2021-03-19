using GenePlanet.HaveIBeenBreached.WebApp;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.Tests.WebApp.RestApiGateway
{
    [TestFixture]
    public class BreachedEmailsControllerTests
    {
        private const string BaseUri = "/breachedemails";
        private const string ValidEmailAddress = "valid@email.cm";
        
        private readonly WebApplicationFactory<Program> _factory;

        public BreachedEmailsControllerTests()
        {
            _factory = new DbClearingApplicationFactory<Program>(nameof(BreachedEmailsControllerTests));
        }

        [Test]
        public async Task TestHappyPath()
        {
            using var httpClient = _factory.CreateClient()!;
            
            var response = await httpClient.GetAsync($"{BaseUri}/{ValidEmailAddress}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            
            response = await httpClient.PostAsync($"{BaseUri}?emailAddress={ValidEmailAddress}", null!);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            
            response = await httpClient.GetAsync($"{BaseUri}/{ValidEmailAddress}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            response = await httpClient.DeleteAsync($"{BaseUri}/{ValidEmailAddress}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            response = await httpClient.GetAsync($"{BaseUri}/{ValidEmailAddress}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            
            response = await httpClient.DeleteAsync($"{BaseUri}/{ValidEmailAddress}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            response = await httpClient.GetAsync($"{BaseUri}/{ValidEmailAddress}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            
            response = await httpClient.PostAsync($"{BaseUri}?emailAddress={ValidEmailAddress}", null!);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            
            response = await httpClient.GetAsync($"{BaseUri}/{ValidEmailAddress}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            response = await httpClient.PostAsync($"{BaseUri}?emailAddress={ValidEmailAddress}", null!);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            
            response = await httpClient.GetAsync($"{BaseUri}/{ValidEmailAddress}");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}