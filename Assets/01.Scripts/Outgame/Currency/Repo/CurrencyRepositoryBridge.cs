using System;
using _01.Scripts.Interfaces.Currency;
using UnityEngine;

namespace _01.Scripts.Outgame.Currency.Repo
{
    public class CurrencyRepositoryBridge : MonoBehaviour
    {
        private CurrencyRepository _repository;

        public ICurrencyRepository Repository => _repository;

        public event Action OnSaveRequested;

        private void Awake()
        {
            _repository = new CurrencyRepository();
        }

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