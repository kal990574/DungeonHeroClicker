using System;
using UnityEngine;
using _01.Scripts.Interfaces;

namespace _01.Scripts.Ingame.Stage
{
    public class StageManager : MonoBehaviour, IStageProvider, IStageProgressHandler
    {
        [Header("Data")]
        [SerializeField] private StageData _stageData;
        [SerializeField] private StageScalingData _scalingData;

        private int _currentStage = 1;
        private int _currentKillCount;
        private StageStatCalculator _statCalculator;

        public int CurrentStage => _currentStage;
        public int CurrentKillCount => _currentKillCount;
        public int RequiredKillCount => _stageData.MonstersPerStage;
        public bool IsNextMonsterBoss => _currentKillCount + 1 >= RequiredKillCount;

        public IMonsterStatModifier StatCalculator => _statCalculator;

        public event Action<int> OnStageChanged;
        public event Action<int, int> OnKillCountChanged;

        private void Awake()
        {
            _statCalculator = new StageStatCalculator(_scalingData, _stageData);
        }

        private void Start()
        {
            OnStageChanged?.Invoke(_currentStage);
            OnKillCountChanged?.Invoke(_currentKillCount, RequiredKillCount);
        }

        public void OnMonsterKilled()
        {
            _currentKillCount++;
            OnKillCountChanged?.Invoke(_currentKillCount, RequiredKillCount);

            if (_currentKillCount >= RequiredKillCount)
            {
                OnStageCleared();
            }
        }

        public void OnStageCleared()
        {
            _currentStage++;
            _currentKillCount = 0;

            OnStageChanged?.Invoke(_currentStage);
            OnKillCountChanged?.Invoke(_currentKillCount, RequiredKillCount);
        }
    }
}