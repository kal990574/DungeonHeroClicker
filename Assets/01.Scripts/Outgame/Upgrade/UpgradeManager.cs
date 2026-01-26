using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade
{
    public class UpgradeManager : MonoBehaviour
    {
        [Header("Upgrades")]
        [SerializeField] private List<Upgrade> _weaponUpgrades;
        [SerializeField] private List<Upgrade> _companionUpgrades;

        public float TotalClickDamage => _weaponUpgrades.Sum(u => u.CurrentEffect);
        public float TotalAutoDPS => _companionUpgrades.Sum(u => u.CurrentEffect);

        public event Action OnStatsChanged;

        private void OnEnable()
        {
            foreach (var upgrade in _weaponUpgrades)
            {
                upgrade.OnUpgraded += HandleUpgraded;
            }
            
            foreach (var upgrade in _companionUpgrades)
            {
                upgrade.OnUpgraded += HandleUpgraded;
            }
        }

        private void OnDisable()
        {
            foreach (var upgrade in _weaponUpgrades)
            {
                upgrade.OnUpgraded -= HandleUpgraded;
            }

            foreach (var upgrade in _companionUpgrades)
            {
                upgrade.OnUpgraded -= HandleUpgraded;
            }
        }
        
        private void HandleUpgraded(Upgrade upgrade)
        {
            OnStatsChanged?.Invoke();
            Debug.Log($"[UpgradeManager] ClickDmg: {TotalClickDamage}, AutoDPS: {TotalAutoDPS}");
        }
    }
}