using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.EfCoreEmailAddressCollectionAdapter
{
    /// <inheritdoc />
    internal class EfCoreEmailAddressCollection : IEmailAddressCollection
    {
        private readonly IDbContextFactory<BreachedEmailsDbContext> _dbContextFactory;

        public EfCoreEmailAddressCollection(IDbContextFactory<BreachedEmailsDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        /// <inheritdoc />
        public async ValueTask<bool> Contains(EmailAddress emailAddress)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext()!;
            var emailAddressDto = await dbContext.EmailAddresses.AsNoTracking()
                .FirstOrDefaultAsync(dto => dto.Address == emailAddress.Value);
            return emailAddressDto is not null;
        }

        /// <inheritdoc />
        public async ValueTask<Result> Add(EmailAddress emailAddress)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext()!;
            dbContext.EmailAddresses.Add(emailAddress);
            try
            {
                await dbContext.SaveChangesAsync();
            } catch (DbUpdateException e) // TODO: EFCore surfaces db specific errors, find a way to generalize
            {
                return Result.Fail(new EmailAddressAlreadyExistsError().CausedBy(e));
            }

            return Result.Ok()!;
        }

        /// <inheritdoc />
        public async ValueTask<Result> Remove(EmailAddress emailAddress)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext()!;
            dbContext.EmailAddresses.Remove(emailAddress);
            try
            {
                await dbContext.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException e) // TODO: EFCore surfaces db specific errors, find a way to generalize
            {
                return Result.Fail(new EmailAddressNotFoundError().CausedBy(e));
            }

            return Result.Ok()!;
        }

        /// <inheritdoc />
        public async IAsyncEnumerator<EmailAddress> GetAsyncEnumerator(CancellationToken cancellationToken = new())
        {
            await using var dbContext = _dbContextFactory.CreateDbContext()!;
            await foreach (var emailAddressDto in dbContext.EmailAddresses.AsNoTracking().AsAsyncEnumerable()!
                .WithCancellation(cancellationToken))
            {
                yield return EmailAddress.Create(emailAddressDto.Address).Value;
            }
        }
    }
}