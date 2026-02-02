using System;
using _01.Scripts.Interfaces;
using _01.Scripts.Outgame.Account.Manager;
using UnityEngine;

namespace _01.Scripts.Ingame.Stage
{
    public class StageRepositoryBridge : MonoBehaviour
    {
        private StageRepository _repository;

        public IStageRepository Repository
        {
            get
            {
                if (_repository == null)
                {
                    string accountId = AccountManager.Instance.CurrentAccountId;

                    if (string.IsNullOrEmpty(accountId))
                    {
                        Debug.LogError("[StageRepositoryBridge] AccountId is empty. Not logged in?");
                        return null;
                    }

                    _repository = new StageRepository(accountId);
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