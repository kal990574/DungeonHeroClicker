using System;
using System.Collections.Generic;
using _01.Scripts.Core.Utils;
using UnityEngine;

namespace _01.Scripts.Ingame.Hero
{
    public class CompanionManager : MonoBehaviour
    {
        [Header("Companions")]
        [SerializeField] private List<Companion> _companions;

        [Header("Visuals")]
        [SerializeField] private List<CompanionVisual> _companionVisuals;

        public BigNumber TotalDPS
        {
            get
            {
                BigNumber total = BigNumber.Zero;
                foreach (var companion in _companions)
                {
                    total += companion.CurrentDPS;
                }
                return total;
            }
        }
        public IReadOnlyList<Companion> Companions => _companions;
        public IReadOnlyList<CompanionVisual> CompanionVisuals => _companionVisuals;

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

        public void PlayAllAttackAnimations()
        {
            foreach (var visual in _companionVisuals)
            {
                if (visual != null && visual.IsSpawned)
                {
                    visual.PlayAttackAnimation();
                }
            }
        }
    }
}