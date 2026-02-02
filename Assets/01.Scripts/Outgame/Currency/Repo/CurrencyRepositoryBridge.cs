using System;
using _01.Scripts.Interfaces.Currency;
using _01.Scripts.Outgame.Account.Manager;
using UnityEngine;

namespace _01.Scripts.Outgame.Currency.Repo
{
    public class CurrencyRepositoryBridge : MonoBehaviour
    {
        private CurrencyRepository _repository;

        public ICurrencyRepository Repository
        {
            get
            {
                if (_repository == null)
                {
                    string accountId = AccountManager.Instance.CurrentAccountId;

                    if (string.IsNullOrEmpty(accountId))
                    {
                        Debug.LogError("[CurrencyRepositoryBridge] AccountId is empty. Not logged in?");
                        return null;
                    }

                    _repository = new CurrencyRepository(accountId);
                }

                return _repository;
            }
        }

        public event Action OnSaveRequested;

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                OnSaveRequested?.Invoke();
            }
        }

        private void OnApplicationQuit()
        {
            OnSaveRequested?.Invoke();
        }
    }
}