using _01.Scripts.Ingame.Hero;
  using TMPro;
  using UnityEngine;
  using UnityEngine.UI;

  namespace _01.Scripts.UI
  {
      public class CompanionButton : MonoBehaviour
      {
          [Header("References")]
          [SerializeField] private Companion _companion;
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
          }

          private void OnDisable()
          {
              _companion.OnPurchased -= HandleChanged;
              _companion.OnUpgraded -= HandleChanged;
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

          private void UpdateUI()
          {
              if (!_companion.IsPurchased)
              {
                  _nameText.text = $"{_companion.Data.CompanionName} [잠금]";
                  _costText.text = $"{_companion.Data.PurchaseCost:N0} G";
                  _dpsText.text = $"DPS +{_companion.Data.BaseDPS:F0}";
                  _button.interactable = _companion.CanPurchase;
              }
              else
              {
                  _nameText.text = $"{_companion.Data.CompanionName} Lv.{_companion.CurrentLevel}";
                  _costText.text = $"{_companion.UpgradeCost:N0} G";
                  _dpsText.text = $"DPS {_companion.CurrentDPS:F1}";
                  _button.interactable = _companion.CanUpgrade;
              }
          }
      }
  }