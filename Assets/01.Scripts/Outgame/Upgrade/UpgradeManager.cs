using System;
using System.Collections.Generic;
using _01.Scripts.Core.Utils;
using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade
{
    public class UpgradeManager : MonoBehaviour
    {
        [Header("Upgrades")]
        [SerializeField] private List<Upgrade> _weaponUpgrades;

        public BigNumber TotalClickDamage
        {
            get
            {
                BigNumber total = BigNumber.Zero;
                foreach (var upgrade in _weaponUpgrades)
                {
                    total += upgrade.CurrentEffect;
                }
                return total;
            }
        }


        private void OnEnable()
        {
            foreach (var upgrade in _weaponUpgrades)
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
        }
        
        private void HandleUpgraded(Upgrade upgrade)
        {
            Debug.Log($"[UpgradeManager] ClickDmg: {TotalClickDamage}");
        }
    }
}