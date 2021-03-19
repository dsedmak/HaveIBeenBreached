using FluentResults;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.CallerContract
{
    public interface IAddBreachedEmailAddress
    {
        ValueTask<Result> AddBreachedEmailAddress(string emailAddress);
    }
}