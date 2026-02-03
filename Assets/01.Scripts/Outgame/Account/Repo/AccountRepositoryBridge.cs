using _01.Scripts.Interfaces.Account;
using UnityEngine;

namespace _01.Scripts.Outgame.Account.Repo
{
    public class AccountRepositoryBridge : MonoBehaviour, IAccountRepository
    {
        private AccountRepository _repository;

        private AccountRepository Repository
        {
            get
            {
                _repository ??= new AccountRepository();
                return _repository;
            }
        }

        public bool Exists(string accountId) => Repository.Exists(accountId);

        public AccountSaveData Load(string accountId) => Repository.Load(accountId);

        public void Save(AccountSaveData data) => Repository.Save(data);
    }
}