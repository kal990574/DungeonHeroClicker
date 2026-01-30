using System;
using _01.Scripts.Interfaces;
using UnityEngine;

namespace _01.Scripts.Ingame.Stage
{
    public class StageRepositoryBridge : MonoBehaviour
    {
        private StageRepository _repository;

        public IStageRepository Repository => _repository;

        public event Action OnSaveRequested;

        private void Awake()
        {
            _repository = new StageRepository();
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