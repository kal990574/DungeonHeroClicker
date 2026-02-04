namespace _01.Scripts.Outgame.Account.Domain
{
    public readonly struct AccountResult
    {
        public readonly bool Success;
        public readonly string ErrorMessage;
        public readonly string UserId;

        private AccountResult(bool success, string errorMessage, string userId)
        {
            Success = success;
            ErrorMessage = errorMessage;
            UserId = userId;
        }

        public static AccountResult Ok(string userId = null)
        {
            return new AccountResult(true, string.Empty, userId);
        }

        public static AccountResult Fail(string message)
        {
            return new AccountResult(false, message, null);
        }
    }
}