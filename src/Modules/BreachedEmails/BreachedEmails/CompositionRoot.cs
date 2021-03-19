using GenePlanet.HaveIBeenBreached.BreachedEmails.CallerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails
{
    /// <summary>
    ///     The composition root for this module
    /// </summary>
    public static class CompositionRoot
    {
        /// <summary>
        ///     Registers types implemented by this module
        /// </summary>
        /// <param name="serviceCollection">The global collection of services</param>
        public static void AddBreachedEmails(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IAddBreachedEmailAddress, AddBreachedEmailAddressUseCase>();
            serviceCollection.TryAddSingleton<IRemoveBreachedEmailAddress, RemoveBreachedEmailAddressUseCase>();
            serviceCollection.TryAddSingleton<ICheckEmailAddress, CheckEmailAddressUseCase>();
            serviceCollection.TryAddSingleton<IManageBreachedEmailAdresses, ManageBreachedEmailAdressesUseCase>();
        }
    }
}