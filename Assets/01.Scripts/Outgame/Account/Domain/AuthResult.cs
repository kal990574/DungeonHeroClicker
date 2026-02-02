namespace _01.Scripts.Outgame.Account.Domain
{
    public readonly struct AuthResult
    {
        public readonly bool Success;
        public readonly string ErrorMessage;

        private AuthResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public static AuthResult Ok()
        {
            return new AuthResult(true, string.Empty);
        }

        public static AuthResult Fail(string message)
        {
            return new AuthResult(false, message);
        }
    }
}