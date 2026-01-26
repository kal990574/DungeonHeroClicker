using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _01.Scripts.Ingame.Hero
{
    public class CompanionManager : MonoBehaviour
    {
        [Header("Companions")]
        [SerializeField] private List<Companion> _companions;

        public float TotalDPS => _companions.Sum(c => c.CurrentDPS);
        public IReadOnlyList<Companion> Companions => _companions;

        public event Action OnDPSChanged;

        private void OnEnable()
        {
            foreach (var companion in _companions)
            {
                companion.OnPurchased += HandleCompanionChanged;
                companion.OnUpgraded += HandleCompanionChanged;
            }
        }
        private void OnDisable()
        {
            foreach (var companion in _companions)
            {
                companion.OnPurchased -= HandleCompanionChanged;
                companion.OnUpgraded -= HandleCompanionChanged;
            }
        }

        private void HandleCompanionChanged(Companion companion)
        {
            OnDPSChanged?.Invoke();
            Debug.Log($"[CompanionManager] Total DPS: {TotalDPS}");
        }
    }
}