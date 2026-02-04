using Cysharp.Threading.Tasks;
using _01.Scripts.Outgame.Account.Domain;

namespace _01.Scripts.Interfaces.Account
{
    public interface IAccountService
    {
        UniTask<AccountResult> TryLogin(string id, string password);
        UniTask<AccountResult> TryRegister(string id, string password, string passwordConfirm);
        void Logout();
    }
}