using _01.Scripts.Interfaces.Account;
using UnityEngine;

namespace _01.Scripts.Outgame.Account.Repo
{
    public class AccountRepositoryBridge : MonoBehaviour
    {
        private AccountRepository _repository;

        public IAccountRepository Repository
        {
            get
            {
                _repository ??= new AccountRepository();
                return _repository;
            }
        }
    }
}