using System;
using _01.Scripts.Interfaces.Upgrade;
using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade.Repo
{
    public class UpgradeRepositoryBridge : MonoBehaviour
    {
        private UpgradeRepository _repository;

        public IUpgradeRepository Repository => _repository;

        public event Action OnSaveRequested;

        private void Awake()
        {
            _repository = new UpgradeRepository();
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