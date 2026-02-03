using System;
using _01.Scripts.Interfaces;
using _01.Scripts.Outgame.Account.Manager;
using UnityEngine;

namespace _01.Scripts.Ingame.Stage
{
    public class StageManager : MonoBehaviour, IStageProvider, IStageProgressHandler
    {
        [Header("Data")]
        [SerializeField] private StageData _stageData;
        [SerializeField] private StageScalingData _scalingData;

        private IStageRepository _repository;

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
            _repository = new StageRepository(AccountManager.Instance.CurrentAccountId);
            _statCalculator = new StageStatCalculator(_scalingData, _stageData);

            LoadOrDefault();
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
            else
            {
                PersistState();
            }
        }

        public void OnStageCleared()
        {
            _currentStage++;
            _currentKillCount = 0;

            PersistState();

            OnStageChanged?.Invoke(_currentStage);
            OnKillCountChanged?.Invoke(_currentKillCount, RequiredKillCount);
        }

        private void LoadOrDefault()
        {
            var data = _repository.Load();

            if (data != null)
            {
                _currentStage = data.CurrentStage;
                _currentKillCount = data.CurrentKillCount;
            }
        }

        private void PersistState()
        {
            _repository.Save(CreateSaveData());
        }

        private StageSaveData CreateSaveData()
        {
            return new StageSaveData
            {
                CurrentStage = _currentStage,
                CurrentKillCount = _currentKillCount
            };
        }

    }
}