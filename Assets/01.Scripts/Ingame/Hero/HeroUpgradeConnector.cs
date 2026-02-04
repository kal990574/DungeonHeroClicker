using _01.Scripts.Outgame.Upgrade;
using _01.Scripts.Outgame.Upgrade.Domain;
using UnityEngine;

namespace _01.Scripts.Ingame.Hero
{
    public class HeroUpgradeConnector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UpgradeManager _upgradeManager;
        [SerializeField] private HeroVisual _heroVisual;

        [Header("Config")]
        [SerializeField] private string _weaponItemId;

        private void OnEnable()
        {
            _upgradeManager.OnItemUpgraded += HandleItemUpgraded;

            if (_upgradeManager.IsInitialized)
                UpdateVisual();
            else
                _upgradeManager.OnInitialized += HandleInitialized;
        }

        private void OnDisable()
        {
            _upgradeManager.OnItemUpgraded -= HandleItemUpgraded;
            _upgradeManager.OnInitialized -= HandleInitialized;
        }

        private void HandleInitialized()
        {
            UpdateVisual();
        }

        private void HandleItemUpgraded(UpgradeItem item)
        {
            if (item.Id == _weaponItemId)
            {
                UpdateVisual();
            }
        }

        private void UpdateVisual()
        {
            if (_heroVisual == null)
            {
                return;
            }

            var item = _upgradeManager.GetItem(_weaponItemId);
            if (item != null)
            {
                _heroVisual.UpdateVisual(item.CurrentLevel);
            }
        }
    }
}