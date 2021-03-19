using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using Microsoft.Extensions.DependencyInjection;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.InMemoryEmailAddressCollectionAdapter
{
    public static class CompositionRoot
    {
        public static void AddInMemoryEmailAddressCollectionAsDecorator(this IServiceCollection serviceCollection)
        {
            serviceCollection.Decorate<IEmailAddressCollection, InMemoryEmailAddressCollection>();
        }
    }
}