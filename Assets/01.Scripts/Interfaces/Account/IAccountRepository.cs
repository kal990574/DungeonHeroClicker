using Cysharp.Threading.Tasks;
using _01.Scripts.Outgame.Account.Domain;

namespace _01.Scripts.Interfaces.Account
{
    public interface IAccountRepository
    {
        UniTask<AccountResult> Register(string email, string password);
        UniTask<AccountResult> Login(string email, string password);
        void Logout();
    }
}