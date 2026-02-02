using _01.Scripts.Outgame.Account.Repo;

namespace _01.Scripts.Interfaces.Account
{
    public interface IAccountRepository
    {
        bool Exists(string accountId);
        AccountSaveData Load(string accountId);
        void Save(AccountSaveData data);
    }
}