using Cysharp.Threading.Tasks;
using _01.Scripts.Outgame.Account.Domain;

namespace _01.Scripts.Interfaces.Account
{
    public interface IFirebaseAccountRepository
    {
        UniTask<AuthResult> RegisterAsync(string email, string password);
        UniTask<AuthResult> LoginAsync(string email, string password);
        void Logout();
        bool IsLoggedIn { get; }
        string CurrentUserId { get; }
    }
}