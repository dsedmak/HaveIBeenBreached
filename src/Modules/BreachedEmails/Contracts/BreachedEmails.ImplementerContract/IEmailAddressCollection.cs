using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract
{
    public interface IEmailAddressCollection : IAsyncEnumerable<EmailAddress>
    {
        ValueTask<bool> Contains(EmailAddress emailAddress);

        ValueTask<Result> Add(EmailAddress emailAddress);

        ValueTask<Result> Remove(EmailAddress emailAddress);
    }
}