using FluentResults;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.CallerContract
{
    public interface IRemoveBreachedEmailAddress
    {
        ValueTask<Result> RemoveBreachedEmailAddress(string emailAddress);
    }
}