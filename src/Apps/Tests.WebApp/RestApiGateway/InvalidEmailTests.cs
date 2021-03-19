using GenePlanet.HaveIBeenBreached.WebApp;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.Tests.WebApp.RestApiGateway
{
    [TestFixture]
    public class InvalidEmailTests
    {
        private readonly WebApplicationFactory<Program> _factory;

        public InvalidEmailTests()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [Test]
        public async Task GetReturnsBadRequestIfEmailAddressIsInvalid()
        {
            using var httpClient = _factory.CreateClient()!;

            var response = await httpClient.GetAsync("/breachedemails/invalidemail");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task PostReturnsBadRequestIfEmailAddressIsInvalid()
        {
            using var httpClient = _factory.CreateClient()!;

            var response = await httpClient.PostAsync("/breachedemails?emailAddress=invalidemail", null!);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task RemoveReturnsBadRequestIfEmailAddressIsInvalid()
        {
            using var httpClient = _factory.CreateClient()!;

            var response = await httpClient.DeleteAsync("/breachedemails/invalidemail");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}