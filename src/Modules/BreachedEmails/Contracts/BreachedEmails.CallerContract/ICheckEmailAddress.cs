using FluentResults;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.CallerContract
{
    public interface ICheckEmailAddress
    {
        ValueTask<Result<bool>> CheckEmailAddress(string emailAddress);
    }
}