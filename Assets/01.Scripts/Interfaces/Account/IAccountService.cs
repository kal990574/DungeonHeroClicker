using _01.Scripts.Outgame.Account.Domain;

namespace _01.Scripts.Interfaces.Account
{
    public interface IAccountService
    {
        AuthResult TryLogin(string id, string password);
        AuthResult TryRegister(string id, string password, string passwordConfirm);
        void Logout();
    }
}