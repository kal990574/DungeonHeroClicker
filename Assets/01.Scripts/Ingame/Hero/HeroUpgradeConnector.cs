using _01.Scripts.Outgame.Upgrade;
using UnityEngine;

namespace _01.Scripts.Ingame.Hero
{
    public class HeroUpgradeConnector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Upgrade _weaponUpgrade;
        [SerializeField] private HeroVisual _heroVisual;

        private void OnEnable()
        {
            if (_weaponUpgrade != null)
            {
                _weaponUpgrade.OnUpgraded += HandleWeaponUpgraded;
            }

            UpdateVisual();
        }

        private void OnDisable()
        {
            if (_weaponUpgrade != null)
            {
                _weaponUpgrade.OnUpgraded -= HandleWeaponUpgraded;
            }
        }

        private void HandleWeaponUpgraded(Upgrade upgrade)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (_heroVisual != null && _weaponUpgrade != null)
            {
                _heroVisual.UpdateVisual(_weaponUpgrade.CurrentLevel);
            }
        }
    }
}