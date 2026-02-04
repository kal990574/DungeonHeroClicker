using System;
using Cysharp.Threading.Tasks;
using _01.Scripts.Interfaces;
using _01.Scripts.Outgame.Stage.Config;
using _01.Scripts.Outgame.Stage.Domain;
using _01.Scripts.Outgame.Stage.Repo;
using UnityEngine;

namespace _01.Scripts.Outgame.Stage
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

        public bool IsInitialized { get; private set; }

        public event Action OnInitialized;
        public event Action<int> OnStageChanged;
        public event Action<int, int> OnKillCountChanged;

        private async void Awake()
        {
            _repository = new FirebaseStageRepository();
            _statCalculator = new StageStatCalculator(_scalingData, _stageData);

            await LoadOrDefaultAsync();
            IsInitialized = true;
            OnInitialized?.Invoke();
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

        private async UniTask LoadOrDefaultAsync()
        {
            var data = await _repository.Load();

            if (data != null)
            {
                _currentStage = data.CurrentStage;
                _currentKillCount = data.CurrentKillCount;
            }
        }

        private void PersistState()
        {
            _repository.Save(CreateSaveData()).Forget();
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