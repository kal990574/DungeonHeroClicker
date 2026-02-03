using System;
using _01.Scripts.Interfaces.Currency;
using _01.Scripts.Outgame.Account.Manager;
using UnityEngine;

namespace _01.Scripts.Outgame.Currency.Repo
{
    public class CurrencyRepositoryBridge : MonoBehaviour, ICurrencyRepository
    {
        private CurrencyRepository _repository;

        private CurrencyRepository Repository
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

        public void Save(CurrencySaveData data) => Repository?.Save(data);

        public CurrencySaveData Load() => Repository?.Load();

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