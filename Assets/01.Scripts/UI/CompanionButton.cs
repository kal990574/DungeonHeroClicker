using _01.Scripts.Ingame.Hero;
using _01.Scripts.Outgame.Currency;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts.UI
{
    public class CompanionButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Companion _companion;
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _dpsText;

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
            _companion.OnPurchased += HandleChanged;
            _companion.OnUpgraded += HandleChanged;

            if (_goldWallet != null)
            {
                _goldWallet.OnGoldChanged += HandleGoldChanged;
            }
        }

        private void OnDisable()
        {
            _companion.OnPurchased -= HandleChanged;
            _companion.OnUpgraded -= HandleChanged;

            if (_goldWallet != null)
            {
                _goldWallet.OnGoldChanged -= HandleGoldChanged;
            }
        }

        private void OnButtonClick()
        {
            if (!_companion.IsPurchased)
            {
                _companion.Purchase();
            }
            else
            {
                _companion.DoUpgrade();
            }
        }

        private void HandleChanged(Companion companion)
        {
            UpdateUI();
        }

        private void HandleGoldChanged(int gold)
        {
            UpdateInteractable();
        }

        private void UpdateUI()
        {
            if (!_companion.IsPurchased)
            {
                _nameText.text = $"{_companion.Data.CompanionName} [Lock]";
                _costText.text = $"{_companion.Data.PurchaseCost:N0} G";
                _dpsText.text = $"DPS +{_companion.Data.BaseDPS:F0}";
            }
            else
            {
                _nameText.text = $"{_companion.Data.CompanionName} Lv.{_companion.CurrentLevel}";
                _costText.text = $"{_companion.UpgradeCost:N0} G";
                _dpsText.text = $"DPS {_companion.CurrentDPS:F1}";
            }

            UpdateInteractable();
        }

        private void UpdateInteractable()
        {
            _button.interactable = _companion.IsPurchased
                ? _companion.CanUpgrade
                : _companion.CanPurchase;
        }
    }
}