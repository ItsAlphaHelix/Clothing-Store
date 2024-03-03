namespace Clothing_Store.Providers
{
    using Microsoft.AspNetCore.Identity;
    public class IdentityErrorDescriberProvider : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "Паролата трябва да съдържа поне един символ, който не е буква или цифра."
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"Паролата трябва да съдържа поне {length} символа."
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "Паролата трябва да съдържа поне една малка буква от ('а'-'я')."
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "Паролата трябва да съдържа поне една голяма буква от ('А'-'Я')."
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "Паролата трябва да съдържа поне една цифра от ('0'-'9')."
            };
        }
    }
}
