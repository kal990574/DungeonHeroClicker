using System;
using _01.Scripts.Interfaces.Upgrade;
using _01.Scripts.Outgame.Account.Manager;
using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade.Repo
{
    public class UpgradeRepositoryBridge : MonoBehaviour, IUpgradeRepository
    {
        private UpgradeRepository _repository;

        private UpgradeRepository Repository
        {
            get
            {
                if (_repository == null)
                {
                    string accountId = AccountManager.Instance.CurrentAccountId;

                    if (string.IsNullOrEmpty(accountId))
                    {
                        Debug.LogError("[UpgradeRepositoryBridge] AccountId is empty. Not logged in?");
                        return null;
                    }

                    _repository = new UpgradeRepository(accountId);
                }

                return _repository;
            }
        }

        public void Save(UpgradeSaveData data) => Repository?.Save(data);

        public UpgradeSaveData Load() => Repository?.Load();

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