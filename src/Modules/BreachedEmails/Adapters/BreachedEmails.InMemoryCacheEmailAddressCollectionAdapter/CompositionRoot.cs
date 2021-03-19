using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.InMemoryCacheEmailAddressCollectionAdapter
{
    public static class CompositionRoot
    {
        public static void AddInMemoryCacheEmailAddressCollectionAsDecorator(
            this IServiceCollection serviceCollection,
            Action<InMemoryCacheEmailAddressCollectionOptions>? options = default)
        {
            serviceCollection.Configure(options ?? (_ => { }));
            serviceCollection.Decorate<IEmailAddressCollection, InMemoryCacheEmailAddressCollection>();
        }

        public static void AddInMemoryCacheEmailAddressCollectionAsDecorator(
            this IServiceCollection serviceCollection,
            IConfiguration options)
        {
            serviceCollection.Configure<InMemoryCacheEmailAddressCollectionOptions>(options);
            serviceCollection.Decorate<IEmailAddressCollection, InMemoryCacheEmailAddressCollection>();
        }
    }
}