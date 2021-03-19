using GenePlanet.HaveIBeenBreached.BreachedEmails.EfCoreEmailAddressCollectionAdapter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace GenePlanet.HaveIBeenBreached.Tests.WebApp
{
    public class DbClearingApplicationFactory<TEntryPoint>
        : WebApplicationFactory<TEntryPoint> where TEntryPoint: class
    {
        private readonly string _databaseName;

        public DbClearingApplicationFactory(string databaseName)
        {
            _databaseName = databaseName;
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BreachedEmailsDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContextFactory<BreachedEmailsDbContext>(options =>
                {
                    options.UseSqlite($"DataSource={_databaseName}.db");
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                
                var dbContextFactory = scopedServices.GetRequiredService<IDbContextFactory<BreachedEmailsDbContext>>();
                using var dbContext = dbContextFactory.CreateDbContext();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            });
        }
    }
}