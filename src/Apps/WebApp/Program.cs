using GenePlanet.HaveIBeenBreached.BreachedEmails;
using GenePlanet.HaveIBeenBreached.BreachedEmails.EfCoreEmailAddressCollectionAdapter;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.InMemoryCacheEmailAddressCollectionAdapter;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ProbabilisticEmailAddressCollectionAdapter;
using GenePlanet.HaveIBeenBreached.RestApiGateway;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.WebApp
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build()!;
            await InitializeSingletons(host);
            await host.RunAsync()!;
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)!.ConfigureWebHostDefaults(ConfigureWebHost);
        }

        private static void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.Configure(ConfigureRequestPipeline);
            builder.ConfigureServices(ConfigureServices);
        }

        private static void ConfigureServices(WebHostBuilderContext builderContext,
            IServiceCollection serviceCollection)
        {
            var configuration = builderContext.Configuration;

            serviceCollection.AddRestApiGateway();
            serviceCollection.AddEfCoreEmailAddressCollection(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")!));
            serviceCollection.AddInMemoryCacheEmailAddressCollectionAsDecorator(
                configuration.GetSection("BreachedEmails:InMemoryCacheEmailAddressCollectionAdapter")!);
            serviceCollection.AddProbabilisticEmailAddressCollectionAsDecorator(
                configuration.GetSection("BreachedEmails:ProbabilisticEmailAddressCollectionAdapter")!);
            serviceCollection.AddBreachedEmails();
        }

        private static void ConfigureRequestPipeline(WebHostBuilderContext builderContext, IApplicationBuilder app)
        {
            if (builderContext.HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.Map("", module => module.UseRestApiGateway(builderContext));
        }

        private static async ValueTask InitializeSingletons(IHost host)
        {
            using var serviceScope = host.Services!.CreateScope();
            var emailAddressCollection = host.Services.GetRequiredService<IEmailAddressCollection>();
            await foreach (var _ in emailAddressCollection) { }
        }
    }
}