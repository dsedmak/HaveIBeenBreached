using GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract;
using System.ComponentModel.DataAnnotations;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.EfCoreEmailAddressCollectionAdapter
{
    // TODO: add address hash as long Hash to support hash partitioning
    internal class EmailAddressDto
    {
        [Key, StringLength(254)]
        public string Address { get; set; } = null!; // https://docs.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types

        public static implicit operator EmailAddressDto(EmailAddress emailAddress)
        {
            return new() {Address = emailAddress.Value};
        }
    }
}