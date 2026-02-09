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

        private StageProgress _progress = new StageProgress();
        private StageStatCalculator _statCalculator;

        public int CurrentStage => _progress.CurrentStage;
        public int CurrentKillCount => _progress.CurrentKillCount;
        public int RequiredKillCount => _stageData.MonstersPerStage;
        public bool IsNextMonsterBoss => _progress.IsNextBoss(RequiredKillCount);

        public IMonsterStatModifier StatCalculator => _statCalculator;

        public bool IsInitialized { get; private set; }

        public event Action<int> OnStageChanged;
        public event Action<int, int> OnKillCountChanged;

        private async void Awake()
        {
#if UNITY_WEBGL
            _repository = new StageRepository();
#else
            _repository = new FirebaseStageRepository();
#endif
            _statCalculator = new StageStatCalculator(_scalingData, _stageData);

            await LoadOrDefaultAsync();
            IsInitialized = true;
            OnStageChanged?.Invoke(_progress.CurrentStage);
            OnKillCountChanged?.Invoke(_progress.CurrentKillCount, RequiredKillCount);
        }

        public void OnMonsterKilled()
        {
            _progress = _progress.WithKillCountIncremented();
            OnKillCountChanged?.Invoke(_progress.CurrentKillCount, RequiredKillCount);

            if (_progress.IsCleared(RequiredKillCount))
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
            _progress = _progress.WithNextStage();
            PersistState();

            OnStageChanged?.Invoke(_progress.CurrentStage);
            OnKillCountChanged?.Invoke(_progress.CurrentKillCount, RequiredKillCount);
        }

        private async UniTask LoadOrDefaultAsync()
        {
            var data = await _repository.Load();

            if (data != null)
            {
                _progress = new StageProgress(data.CurrentStage, data.CurrentKillCount);
            }
            else
            {
                _progress = new StageProgress();
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
                CurrentStage = _progress.CurrentStage,
                CurrentKillCount = _progress.CurrentKillCount
            };
        }

    }
}