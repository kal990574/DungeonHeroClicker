using System;
using _01.Scripts.Core.Utils;
using _01.Scripts.Interfaces;
using _01.Scripts.Outgame.Currency;
using _01.Scripts.Core.Audio;
using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade
{
    public class Upgrade : MonoBehaviour, IUpgradeable
    {
        [Header("Data")]
        [SerializeField] private UpgradeData _data;

        [Header("Dependencies")]
        [SerializeField] private GoldWallet _goldWallet;

        private int _currentLevel;

        public UpgradeData Data => _data;
        public int CurrentLevel => _currentLevel;
        public BigNumber UpgradeCost => _data.GetCost(_currentLevel);
        public float CurrentEffect => _data.GetEffect(_currentLevel);
        public bool CanUpgrade => _goldWallet.CanAfford(UpgradeCost);

        public event Action<Upgrade> OnUpgraded;

        public void DoUpgrade()
        {
            if (!CanUpgrade)
            {
                return;
            }

            _goldWallet.Spend(UpgradeCost);
            _currentLevel++;
            SFXManager.Instance?.PlayUpgrade();
            OnUpgraded?.Invoke(this);

            Debug.Log($"[Upgrade] {_data.UpgradeName} Lv.{_currentLevel}, Effect: {CurrentEffect}");
        }
    }
}