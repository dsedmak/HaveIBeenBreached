using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.EfCoreEmailAddressCollectionAdapter
{
    public static class CompositionRoot
    {
        public static void AddEfCoreEmailAddressCollection(
            this IServiceCollection serviceCollection,
            Action<DbContextOptionsBuilder> configureDbContext)
        {
            serviceCollection.AddDbContextFactory<BreachedEmailsDbContext>(configureDbContext);
            serviceCollection.TryAddSingleton<IEmailAddressCollection, EfCoreEmailAddressCollection>();
        }
    }
}