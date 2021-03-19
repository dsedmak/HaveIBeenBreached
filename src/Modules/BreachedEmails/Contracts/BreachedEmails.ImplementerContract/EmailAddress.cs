using FluentResults;
using GenePlanet.HaveIBeenBreached.BreachedEmails.SharedContract.Errors;
using System;
using System.Text;

namespace GenePlanet.HaveIBeenBreached.BreachedEmails.ImplementerContract
{
    public sealed record EmailAddress
    {
        private EmailAddress(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<EmailAddress> Create(string? value)
        {
            if (!string.IsNullOrWhiteSpace(value) && ValueContainsExactlyOneAtSign(value))
            {
                var byteCount = Encoding.UTF8.GetByteCount(value);
                if (byteCount < 254) // https://stackoverflow.com/a/574698
                {
                    return Result.Ok(new EmailAddress(value))!;
                }
            }

            return Result.Fail(new InvalidEmailError());
        }

        private static bool ValueContainsExactlyOneAtSign(string stringValue)
        {
            var index = stringValue.IndexOf('@', StringComparison.Ordinal);
            return index > 0 &&
                   index != stringValue.Length - 1 &&
                   index == stringValue.LastIndexOf('@');
        }
    }
}