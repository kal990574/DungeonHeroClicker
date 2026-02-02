using System;
using _01.Scripts.Core.Audio;
using _01.Scripts.Core.Utils;
using _01.Scripts.Ingame.Monster;
using _01.Scripts.Interfaces.Currency;
using _01.Scripts.Outgame.Currency.Config;
using _01.Scripts.Outgame.Currency.Domain;
using _01.Scripts.Outgame.Currency.Repo;
using _01.Scripts.UI.Effects;
using UnityEngine;

namespace _01.Scripts.Outgame.Currency
{
    public class CurrencyManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private CurrencyConfig[] _configs;

        [Header("Dependencies")]
        [SerializeField] private CurrencyRepositoryBridge _repositoryBridge;

        [Header("Effects")]
        [SerializeField] private GoldFlyEffect _goldFlyEffect;

        private ICurrencyRepository _repository;
        private Domain.Currency[] _currencies;


        public event Action<ECurrencyType> OnCurrencyChanged;


        public Domain.Currency Gold => _currencies[(int)ECurrencyType.Gold];

        public Domain.Currency Get(ECurrencyType type)
        {
            return _currencies[(int)type];
        }

        public bool CanAfford(ECurrencyType type, BigNumber cost)
        {
            return _currencies[(int)type].Value >= cost;
        }

        public bool CanAffordGold(BigNumber cost)
        {
            return CanAfford(ECurrencyType.Gold, cost);
        }

        public void AddGold(BigNumber amount)
        {
            Add(ECurrencyType.Gold, amount);
        }

        public bool TrySpendGold(BigNumber cost)
        {
            return TrySpend(ECurrencyType.Gold, cost);
        }

        public void Add(ECurrencyType type, BigNumber amount)
        {
            if (amount <= BigNumber.Zero)
            {
                return;
            }

            int index = (int)type;
            _currencies[index] = new Domain.Currency(_currencies[index].Value + amount);
            PersistState();
            OnCurrencyChanged?.Invoke(type);
        }

        public bool TrySpend(ECurrencyType type, BigNumber cost)
        {
            if (!CanAfford(type, cost))
            {
                return false;
            }

            int index = (int)type;
            _currencies[index] = new Domain.Currency(_currencies[index].Value - cost);
            PersistState();
            OnCurrencyChanged?.Invoke(type);
            return true;
        }

        private void Awake()
        {
            _repository = _repositoryBridge.Repository;
            _currencies = new Domain.Currency[(int)ECurrencyType.Count];

            LoadOrDefault();

            _repositoryBridge.OnSaveRequested += HandleSaveRequested;
        }

        private void OnEnable()
        {
            MonsterReward.OnGoldDropped += HandleGoldDropped;
        }

        private void OnDisable()
        {
            MonsterReward.OnGoldDropped -= HandleGoldDropped;
        }

        private void OnDestroy()
        {
            _repositoryBridge.OnSaveRequested -= HandleSaveRequested;
        }

        private void LoadOrDefault()
        {
            var data = _repository.Load();

            if (data != null)
            {
                int count = (int)ECurrencyType.Count;
                for (int i = 0; i < count; i++)
                {
                    _currencies[i] = new Domain.Currency(
                        new BigNumber(data.Mantissas[i], data.Exponents[i])
                    );
                }
            }
            else
            {
                foreach (var config in _configs)
                {
                    _currencies[(int)config.Type] = new Domain.Currency(
                        new BigNumber(config.InitialAmount)
                    );
                }
            }
        }

        private void PersistState()
        {
            _repository.Save(CreateSaveData());
        }

        private CurrencySaveData CreateSaveData()
        {
            int count = (int)ECurrencyType.Count;
            var data = new CurrencySaveData
            {
                Mantissas = new double[count],
                Exponents = new long[count]
            };

            for (int i = 0; i < count; i++)
            {
                data.Mantissas[i] = _currencies[i].Value.Mantissa;
                data.Exponents[i] = _currencies[i].Value.Exponent;
            }

            return data;
        }

        private void HandleSaveRequested()
        {
            PersistState();
        }

        private void HandleGoldDropped(BigNumber amount, Vector3 worldPosition)
        {
            if (_goldFlyEffect != null)
            {
                _goldFlyEffect.Play(worldPosition, amount, OnCoinArrived);
            }
            else
            {
                AddGold(amount);
                SFXManager.Instance?.PlayCoin();
            }
        }

        private void OnCoinArrived(BigNumber goldValue)
        {
            AddGold(goldValue);
            SFXManager.Instance?.PlayCoin();
        }
    }
}