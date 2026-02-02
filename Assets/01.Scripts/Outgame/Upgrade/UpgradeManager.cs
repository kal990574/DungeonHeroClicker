using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private UpgradeRepositoryBridge _repositoryBridge;
        [SerializeField] private CurrencyManager _currencyManager;

        private IUpgradeRepository _repository;
        private readonly Dictionary<string, UpgradeItem> _items = new();

        // Events.
        public event Action<UpgradeItem> OnItemUpgraded;
        public event Action<UpgradeItem> OnItemPurchased;
        public event Action OnTotalDPSChanged;

        private void Awake()
        {
            _repository = _repositoryBridge.Repository;

            InitializeItems();

            _repositoryBridge.OnSaveRequested += HandleSaveRequested;
        }

        private void OnDestroy()
        {
            _repositoryBridge.OnSaveRequested -= HandleSaveRequested;
        }

        private void InitializeItems()
        {
            var saveData = _repository.Load();
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

        // --- Query API ---

        public UpgradeItem GetItem(string id)
        {
            return _items.TryGetValue(id, out var item) ? item : null;
        }

        public IReadOnlyList<UpgradeItem> GetAllItems()
        {
            return _items.Values.ToList();
        }

        public IReadOnlyList<UpgradeItem> GetItemsByType(EUpgradeType type)
        {
            return _items.Values.Where(i => i.Type == type).ToList();
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
                        total += item.CurrentEffect;
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
                        total += item.CurrentEffect;
                    }
                }
                return total;
            }
        }

        // --- Command API ---

        public bool TryUpgrade(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null)
            {
                return false;
            }

            if (!item.CanUpgrade())
            {
                return false;
            }

            BigNumber cost = item.UpgradeCost;
            if (!_currencyManager.TrySpendGold(cost))
            {
                return false;
            }

            item.TryUpgrade();
            PersistState();

            SFXManager.Instance?.PlayUpgrade();
            OnItemUpgraded?.Invoke(item);
            NotifyTypeChanged(item.Type);

            Debug.Log($"[UpgradeManager] {item.DisplayName} Lv.{item.CurrentLevel}, Effect: {item.CurrentEffect}");
            return true;
        }

        public bool TryPurchase(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null)
            {
                return false;
            }

            if (!item.CanPurchase())
            {
                return false;
            }

            BigNumber cost = item.PurchaseCost;
            if (!_currencyManager.TrySpendGold(cost))
            {
                return false;
            }

            item.TryPurchase();
            PersistState();

            SFXManager.Instance?.PlayUpgrade();
            OnItemPurchased?.Invoke(item);
            NotifyTypeChanged(item.Type);

            Debug.Log($"[UpgradeManager] {item.DisplayName} Purchased! Effect: {item.CurrentEffect}");
            return true;
        }

        // --- Private ---

        private void NotifyTypeChanged(EUpgradeType type)
        {
            if (type == EUpgradeType.Companion)
            {
                OnTotalDPSChanged?.Invoke();
            }
        }

        private void PersistState()
        {
            _repository.Save(CreateSaveData());
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

        private void HandleSaveRequested()
        {
            PersistState();
        }
    }
}