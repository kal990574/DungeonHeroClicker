using System;
using _01.Scripts.Outgame.Currency;
using UnityEngine;
using _01.Scripts.Core.Audio;

namespace _01.Scripts.Ingame.Hero
{
    public class Companion : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private CompanionData _data;

        [Header("Dependencies")]
        [SerializeField] private GoldWallet _goldWallet;

        private bool _isPurchased;
        private int _currentLevel;

        public CompanionData Data => _data;
        public bool IsPurchased => _isPurchased;
        public int CurrentLevel => _currentLevel;
        public float CurrentDPS => _isPurchased ? _data.GetDPS(_currentLevel) : 0f;
        public int UpgradeCost => _data.GetUpgradeCost(_currentLevel);
        public bool CanPurchase => !_isPurchased && _goldWallet.CanAfford(_data.PurchaseCost);
        public bool CanUpgrade => _isPurchased && _goldWallet.CanAfford(UpgradeCost);

        public event Action<Companion> OnPurchased;
        public event Action<Companion> OnUpgraded;

        public void Purchase()
        {
            if (!CanPurchase)
            {
                return;
            }
            
            _goldWallet.Spend(_data.PurchaseCost);
            _isPurchased = true;
            _currentLevel = 1;
            SFXManager.Instance?.PlayUpgrade();
            
            OnPurchased?.Invoke(this);
            Debug.Log($"[Companion] {_data.CompanionName} Purchased! DPS: {CurrentDPS}");
        }

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
            Debug.Log($"[Companion] {_data.CompanionName} Lv.{_currentLevel}, DPS: {CurrentDPS}");
        }
    }
}