using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.ProbabilisticEmailAddressCollectionAdapter
{
    public static class CompositionRoot
    {
        public static void AddProbabilisticEmailAddressCollection(
            this IServiceCollection serviceCollection,
            Action<ProbabilisticEmailAddressCollectionOptions>? options = default)
        {
            serviceCollection.Configure(options ?? (_ => { }));
            serviceCollection.TryAddSingleton<IEmailAddressCollection, BloomFilterEmailAddressCollection>();
        }

        public static void AddProbabilisticEmailAddressCollection(
            this IServiceCollection serviceCollection,
            IConfiguration options)
        {
            serviceCollection.Configure<ProbabilisticEmailAddressCollectionOptions>(options);
            serviceCollection.TryAddSingleton<IEmailAddressCollection, BloomFilterEmailAddressCollection>();
        }

        public static void AddProbabilisticEmailAddressCollectionAsDecorator(
            this IServiceCollection serviceCollection,
            Action<ProbabilisticEmailAddressCollectionOptions>? options = default)
        {
            serviceCollection.Configure(options ?? (_ => { }));
            serviceCollection.Decorate<IEmailAddressCollection, BloomFilterEmailAddressCollection>();
        }

        public static void AddProbabilisticEmailAddressCollectionAsDecorator(
            this IServiceCollection serviceCollection,
            IConfiguration options)
        {
            serviceCollection.Configure<ProbabilisticEmailAddressCollectionOptions>(options);
            serviceCollection.Decorate<IEmailAddressCollection, BloomFilterEmailAddressCollection>();
        }
    }
}