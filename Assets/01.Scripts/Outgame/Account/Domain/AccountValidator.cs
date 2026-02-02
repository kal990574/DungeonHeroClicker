namespace _01.Scripts.Outgame.Account.Domain
{
    public static class AccountValidator
    {
        private const int MinPasswordLength = 6;
        private const int MaxPasswordLength = 15;

        public static AuthResult ValidateId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return AuthResult.Fail("Please enter your ID.");
            }

            return AuthResult.Ok();
        }

        public static AuthResult ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return AuthResult.Fail("Please enter your password.");
            }

            if (password.Length < MinPasswordLength || password.Length > MaxPasswordLength)
            {
                return AuthResult.Fail($"Password must be between {MinPasswordLength} and {MaxPasswordLength} characters.");
            }

            return AuthResult.Ok();
        }

        public static AuthResult ValidatePasswordConfirm(string password, string passwordConfirm)
        {
            if (string.IsNullOrEmpty(passwordConfirm) || password != passwordConfirm)
            {
                return AuthResult.Fail("Passwords do not match.");
            }

            return AuthResult.Ok();
        }
    }
}