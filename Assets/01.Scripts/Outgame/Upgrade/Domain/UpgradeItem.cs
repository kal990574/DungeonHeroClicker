using System;
using _01.Scripts.Core.Utils;
using _01.Scripts.Outgame.Upgrade.Config;
using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade.Domain
{
    public class UpgradeItem
    {
        private readonly UpgradeConfigBase _config;
        private int _currentLevel;
        private bool _isPurchased;

        public string Id => _config.Id;
        public string DisplayName => _config.DisplayName;
        public Sprite Icon => _config.Icon;
        public EUpgradeType Type => _config.Type;

        public int CurrentLevel => _currentLevel;
        public bool IsPurchased => _isPurchased;

        public BigNumber UpgradeCost
        {
            get
            {
                double cost = _config.BaseCost * Math.Pow(_config.CostMultiplier, _currentLevel);
                return new BigNumber(Math.Round(cost));
            }
        }

        public BigNumber CurrentEffect
        {
            get
            {
                double effect = _config.BaseEffect * Math.Pow(_config.EffectMultiplier, _currentLevel);
                return new BigNumber(effect);
            }
        }

        public bool RequiresPurchase => _config.Type == EUpgradeType.Companion;

        public BigNumber PurchaseCost
        {
            get
            {
                if (_config is CompanionUpgradeConfig companion)
                {
                    return companion.PurchaseCost;
                }

                return BigNumber.Zero;
            }
        }

        public UpgradeItem(UpgradeConfigBase config, int level, bool isPurchased)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (string.IsNullOrEmpty(config.Id))
            {
                throw new ArgumentException("ID는 비어있을 수 없습니다.");
            }

            if (level < 0)
            {
                throw new ArgumentException($"레벨은 0 이상이어야 합니다: {config.Id}");
            }

            if (config.BaseCost <= 0)
            {
                throw new ArgumentException($"기본 비용은 0보다 커야 합니다: {config.Id}");
            }

            if (config.BaseEffect <= 0)
            {
                throw new ArgumentException($"기본 효과는 0보다 커야 합니다: {config.Id}");
            }

            if (config.CostMultiplier <= 0)
            {
                throw new ArgumentException($"비용 증가량은 0보다 커야 합니다: {config.Id}");
            }

            if (config.EffectMultiplier <= 0)
            {
                throw new ArgumentException($"효과 증가량은 0보다 커야 합니다: {config.Id}");
            }

            _config = config;
            _currentLevel = level;
            _isPurchased = isPurchased;
        }

        public bool CanUpgrade()
        {
            return _isPurchased;
        }

        public bool TryUpgrade()
        {
            if (!CanUpgrade())
            {
                return false;
            }

            _currentLevel++;
            return true;
        }

        public bool CanPurchase()
        {
            return RequiresPurchase && !_isPurchased;
        }

        public bool TryPurchase()
        {
            if (!CanPurchase())
            {
                return false;
            }

            _isPurchased = true;
            _currentLevel = 1;
            return true;
        }
    }
}