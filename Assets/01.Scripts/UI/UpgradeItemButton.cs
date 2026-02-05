using _01.Scripts.Core.Audio;
using _01.Scripts.Core.Utils;
using _01.Scripts.Outgame.Currency;
using _01.Scripts.Outgame.Currency.Domain;
using _01.Scripts.Outgame.Upgrade;
using _01.Scripts.Outgame.Upgrade.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts.UI
{
    public class UpgradeItemButton : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private string _upgradeItemId;

        [Header("Dependencies")]
        [SerializeField] private UpgradeManager _upgradeManager;
        [SerializeField] private CurrencyManager _currencyManager;

        [Header("UI Elements")]
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _effectText;

        private UpgradeItem _item;

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);

            if (_upgradeManager.IsInitialized)
                InitializeItem();
            else
                _upgradeManager.OnInitialized += HandleInitialized;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnEnable()
        {
            _upgradeManager.OnItemUpgraded += HandleItemChanged;
            _upgradeManager.OnItemPurchased += HandleItemChanged;
            _currencyManager.OnCurrencyChanged += HandleCurrencyChanged;

            UpdateUI();
        }

        private void OnDisable()
        {
            _upgradeManager.OnItemUpgraded -= HandleItemChanged;
            _upgradeManager.OnItemPurchased -= HandleItemChanged;
            _upgradeManager.OnInitialized -= HandleInitialized;
            _currencyManager.OnCurrencyChanged -= HandleCurrencyChanged;
        }

        private void HandleInitialized()
        {
            InitializeItem();
        }

        private void InitializeItem()
        {
            _item = _upgradeManager.GetItem(_upgradeItemId);
            UpdateUI();
        }

        private void OnButtonClick()
        {
            if (_item == null)
            {
                return;
            }

            if (_upgradeManager.RequiresPurchase(_item) && !_item.IsPurchased)
            {
                _upgradeManager.TryPurchase(_item.Id);
            }
            else
            {
                _upgradeManager.TryUpgrade(_item.Id);
            }

            SFXManager.Instance?.PlayUI();
        }

        private void HandleItemChanged(UpgradeItem item)
        {
            if (item.Id == _upgradeItemId)
            {
                _item = item;
                UpdateUI();
            }
        }

        private void HandleCurrencyChanged(ECurrencyType type)
        {
            if (type == ECurrencyType.Gold)
            {
                UpdateInteractable();
            }
        }

        private void UpdateUI()
        {
            if (_item == null)
            {
                return;
            }

            if (_upgradeManager.RequiresPurchase(_item) && !_item.IsPurchased)
            {
                ShowPurchaseView();
            }
            else
            {
                ShowUpgradeView();
            }

            UpdateInteractable();
        }

        private void ShowPurchaseView()
        {
            _nameText.text = $"{_item.DisplayName} [Lock]";
            _costText.text = $"{NumberFormatter.Format(_upgradeManager.GetPurchaseCost(_item))} G";

            string effectLabel = _item.Type == EUpgradeType.Companion ? "DPS" : "DMG";
            _effectText.text = $"{effectLabel} +{NumberFormatter.Format(_upgradeManager.GetCurrentEffect(_item))}";
        }

        private void ShowUpgradeView()
        {
            _nameText.text = $"{_item.DisplayName} Lv.{_item.CurrentLevel}";
            _costText.text = $"{NumberFormatter.Format(_upgradeManager.GetUpgradeCost(_item))} G";

            string effectLabel = _item.Type == EUpgradeType.Companion ? "DPS" : "DMG";
            _effectText.text = $"{effectLabel} {NumberFormatter.Format(_upgradeManager.GetCurrentEffect(_item))}";
        }

        private void UpdateInteractable()
        {
            if (_item == null)
            {
                _button.interactable = false;
                return;
            }

            if (_upgradeManager.RequiresPurchase(_item) && !_item.IsPurchased)
            {
                _button.interactable = _upgradeManager.CanPurchase(_item.Id);
            }
            else
            {
                _button.interactable = _upgradeManager.CanUpgrade(_item.Id);
            }
        }
    }
}