using UnityEngine;
using System;
using _01.Scripts.Core.Utils;

namespace _01.Scripts.Outgame.Currency
{
    public class GoldWallet : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private long _initialGold;

        private BigNumber _currentGold;

        public BigNumber CurrentGold => _currentGold;

        public event Action<BigNumber> OnGoldChanged;

        private void Awake()
        {
            _currentGold = new BigNumber(_initialGold);
        }

        public void Add(BigNumber amount)
        {
            if (amount <= BigNumber.Zero)
            {
                return;
            }

            _currentGold += amount;
            OnGoldChanged?.Invoke(_currentGold);
        }

        public bool CanAfford(BigNumber cost)
        {
            return _currentGold >= cost;
        }

        public bool Spend(BigNumber amount)
        {
            if (!CanAfford(amount))
            {
                return false;
            }

            _currentGold -= amount;
            OnGoldChanged?.Invoke(_currentGold);
            return true;
        }
    }
}