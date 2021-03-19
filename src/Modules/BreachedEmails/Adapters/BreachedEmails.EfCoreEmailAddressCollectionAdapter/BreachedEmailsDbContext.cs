using Microsoft.EntityFrameworkCore;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.EfCoreEmailAddressCollectionAdapter
{
    internal class BreachedEmailsDbContext : DbContext
    {
        public BreachedEmailsDbContext(DbContextOptions<BreachedEmailsDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmailAddressDto> EmailAddresses => Set<EmailAddressDto>()!;
    }
}