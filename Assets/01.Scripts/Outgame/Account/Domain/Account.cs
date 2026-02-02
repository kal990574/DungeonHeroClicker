namespace _01.Scripts.Outgame.Account.Domain
{
    public class Account
    {
        public readonly string Id;
        public readonly string Password;

        public Account(string id, string password)
        {
            Id = id;
            Password = password;
        }
    }
}