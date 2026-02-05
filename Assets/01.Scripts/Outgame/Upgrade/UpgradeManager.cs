using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using _01.Scripts.Core.Audio;
using _01.Scripts.Core.Utils;
using _01.Scripts.Interfaces.Upgrade;
using _01.Scripts.Outgame.Currency;
using _01.Scripts.Outgame.Upgrade.Config;
using _01.Scripts.Outgame.Upgrade.Domain;
using _01.Scripts.Outgame.Upgrade.Repo;
using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade
{
    public class UpgradeManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private List<UpgradeConfigBase> _upgradeConfigs;

        [Header("Dependencies")]
        [SerializeField] private CurrencyManager _currencyManager;

        private IUpgradeRepository _repository;

        private readonly Dictionary<string, UpgradeItem> _items = new();

        public bool IsInitialized { get; private set; }

        public event Action OnInitialized;
        public event Action<UpgradeItem> OnItemUpgraded;
        public event Action<UpgradeItem> OnItemPurchased;
        public event Action OnTotalDPSChanged;

        private async void Awake()
        {
            _repository = new FirebaseUpgradeRepository();
            await InitializeItemsAsync();
            IsInitialized = true;
            OnInitialized?.Invoke();
        }

        private async UniTask InitializeItemsAsync()
        {
            var saveData = await _repository.Load();
            var savedEntries = new Dictionary<string, UpgradeStateEntry>();

            if (saveData?.Entries != null)
            {
                foreach (var entry in saveData.Entries)
                {
                    savedEntries[entry.Id] = entry;
                }
            }

            foreach (var config in _upgradeConfigs)
            {
                int level = 0;
                bool isPurchased = false;

                if (savedEntries.TryGetValue(config.Id, out var entry))
                {
                    level = entry.CurrentLevel;
                    isPurchased = entry.IsPurchased;
                }

                var item = new UpgradeItem(config, level, isPurchased);
                _items[config.Id] = item;
            }
        }

        public UpgradeItem GetItem(string id)
        {
            return _items.TryGetValue(id, out var item) ? item : null;
        }

        public BigNumber GetUpgradeCost(UpgradeItem item)
        {
            double cost = item.Config.BaseCost * Math.Pow(item.Config.CostMultiplier, item.CurrentLevel);
            return new BigNumber(Math.Round(cost));
        }

        public BigNumber GetCurrentEffect(UpgradeItem item)
        {
            double effect = item.Config.BaseEffect * Math.Pow(item.Config.EffectMultiplier, item.CurrentLevel);
            return new BigNumber(effect);
        }

        public BigNumber GetPurchaseCost(UpgradeItem item)
        {
            if (item.Config is CompanionUpgradeConfig companion)
            {
                return companion.PurchaseCost;
            }

            return BigNumber.Zero;
        }

        public bool RequiresPurchase(UpgradeItem item)
        {
            return item.Type == EUpgradeType.Companion;
        }

        public BigNumber TotalClickDamage
        {
            get
            {
                BigNumber total = BigNumber.Zero;
                foreach (var item in _items.Values)
                {
                    if (item.Type == EUpgradeType.Hero && item.IsPurchased)
                    {
                        total += GetCurrentEffect(item);
                    }
                }
                return total;
            }
        }

        public BigNumber TotalDPS
        {
            get
            {
                BigNumber total = BigNumber.Zero;
                foreach (var item in _items.Values)
                {
                    if (item.Type == EUpgradeType.Companion && item.IsPurchased)
                    {
                        total += GetCurrentEffect(item);
                    }
                }
                return total;
            }
        }

        public bool CanUpgrade(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null)
            {
                return false;
            }

            return item.IsPurchased && _currencyManager.CanAffordGold(GetUpgradeCost(item));
        }

        public bool CanPurchase(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null)
            {
                return false;
            }

            return RequiresPurchase(item) && !item.IsPurchased && _currencyManager.CanAffordGold(GetPurchaseCost(item));
        }

        public bool TryUpgrade(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null || !item.IsPurchased)
            {
                return false;
            }

            BigNumber cost = GetUpgradeCost(item);
            if (!_currencyManager.TrySpendGold(cost))
            {
                return false;
            }

            var upgradedItem = new UpgradeItem(item.Config, item.CurrentLevel + 1, item.IsPurchased);
            _items[itemId] = upgradedItem;
            PersistState();

            SFXManager.Instance?.PlayUpgrade();
            OnItemUpgraded?.Invoke(upgradedItem);
            NotifyTypeChanged(upgradedItem.Type);

            Debug.Log($"[UpgradeManager] {upgradedItem.DisplayName} Lv.{upgradedItem.CurrentLevel}, Effect: {GetCurrentEffect(upgradedItem)}");
            return true;
        }

        public bool TryPurchase(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null || !RequiresPurchase(item) || item.IsPurchased)
            {
                return false;
            }

            BigNumber cost = GetPurchaseCost(item);
            if (!_currencyManager.TrySpendGold(cost))
            {
                return false;
            }

            var purchasedItem = new UpgradeItem(item.Config, 1, true);
            _items[itemId] = purchasedItem;
            PersistState();

            SFXManager.Instance?.PlayUpgrade();
            OnItemPurchased?.Invoke(purchasedItem);
            NotifyTypeChanged(purchasedItem.Type);

            Debug.Log($"[UpgradeManager] {purchasedItem.DisplayName} Purchased! Effect: {GetCurrentEffect(purchasedItem)}");
            return true;
        }

        private void NotifyTypeChanged(EUpgradeType type)
        {
            if (type == EUpgradeType.Companion)
            {
                OnTotalDPSChanged?.Invoke();
            }
        }

        private void PersistState()
        {
            _repository.Save(CreateSaveData()).Forget();
        }

        private UpgradeSaveData CreateSaveData()
        {
            var data = new UpgradeSaveData();

            foreach (var item in _items.Values)
            {
                data.Entries.Add(new UpgradeStateEntry
                {
                    Id = item.Id,
                    Type = (int)item.Type,
                    CurrentLevel = item.CurrentLevel,
                    IsPurchased = item.IsPurchased
                });
            }

            return data;
        }

    }
}