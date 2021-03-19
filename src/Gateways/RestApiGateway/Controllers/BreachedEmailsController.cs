using GenePlanet.HaveIBeenBreached.BreachedEmails.CallerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.RestApiGateway.Controllers
{
    [ApiController]
    [Route("breachedemails")]
    public class BreachedEmailsController : ControllerBase
    {
        private readonly IManageBreachedEmailAdresses _breachedEmailAdresses;

        public BreachedEmailsController(IManageBreachedEmailAdresses breachedEmailAdresses)
        {
            _breachedEmailAdresses = breachedEmailAdresses;
        }

        [HttpGet("{emailAddress}")]
        public async Task<ActionResult> Get(string emailAddress)
        {
            var result = await _breachedEmailAdresses.CheckEmailAddress(emailAddress);
            return result.IsSuccess ? result.Value ? Ok() : NotFound() : BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> Post(string emailAddress)
        {
            var result = await _breachedEmailAdresses.AddBreachedEmailAddress(emailAddress);
            return result.IsSuccess ? Created(Url!.Action("Get", new {emailAddress})!, emailAddress) :
                result.HasError<EmailAddressAlreadyExistsError>() ? Conflict() : BadRequest();
        }

        [HttpDelete("{emailAddress}")]
        public async Task<ActionResult> Delete(string emailAddress)
        {
            var result = await _breachedEmailAdresses.RemoveBreachedEmailAddress(emailAddress);
            return result.IsSuccess ? Ok() : BadRequest();
        }
    }
}