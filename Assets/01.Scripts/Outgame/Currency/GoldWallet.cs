using UnityEngine;
using System;

namespace _01.Scripts.Outgame.Currency
{
    public class GoldWallet : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _initialGold;

        private int _currentGold;

        public int CurrentGold => _currentGold;

        public event Action<int> OnGoldChanged;

        private void Awake()
        {
            _currentGold = _initialGold;
        }

        public void Add(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            _currentGold += amount;
            OnGoldChanged?.Invoke(amount);
        }

        public bool CanAfford(int cost)
        {
            return _currentGold >= cost;
        }

        public bool Spend(int amount)
        {
            if (!CanAfford(amount))
            {
                return false;
            }

            _currentGold -= amount;
            OnGoldChanged?.Invoke(amount);
            return true;
        }
    }
}