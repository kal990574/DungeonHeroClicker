using _01.Scripts.Outgame.Currency;
using _01.Scripts.Outgame.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _01.Scripts.Core.Audio;

namespace _01.Scripts.UI
{
    public class UpgradeButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Upgrade _upgrade;
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _effectText;

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
            UpdateUI();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnEnable()
        {
            _upgrade.OnUpgraded += HandleUpgraded;

            if (_goldWallet != null)
            {
                _goldWallet.OnGoldChanged += HandleGoldChanged;
            }
        }

        private void OnDisable()
        {
            _upgrade.OnUpgraded -= HandleUpgraded;

            if (_goldWallet != null)
            {
                _goldWallet.OnGoldChanged -= HandleGoldChanged;
            }
        }

        private void OnButtonClick()
        {
            _upgrade.DoUpgrade();
            SFXManager.Instance.PlayUI();
        }

        private void HandleUpgraded(Upgrade upgrade)
        {
            UpdateUI();
        }

        private void HandleGoldChanged(int gold)
        {
            _button.interactable = _upgrade.CanUpgrade;
        }

        private void UpdateUI()
        {
            _nameText.text = $"{_upgrade.Data.UpgradeName} Lv.{_upgrade.CurrentLevel}";
            _costText.text = $"{_upgrade.UpgradeCost:N0} G";
            _effectText.text = $"DMG +{_upgrade.CurrentEffect:F0}";
            _button.interactable = _upgrade.CanUpgrade;
        }
    }
}