using _01.Scripts.Core.Utils;
using _01.Scripts.Outgame.Currency;
using TMPro;
using UnityEngine;

namespace _01.Scripts.UI
{
    public class GoldDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private TMP_Text _goldText;

        private void OnEnable()
        {
            _goldWallet.OnGoldChanged += UpdateDisplay;
            UpdateDisplay(_goldWallet.CurrentGold);
        }

        private void OnDisable()
        {
            _goldWallet.OnGoldChanged -= UpdateDisplay;
        }

        private void UpdateDisplay(int gold)
        {
            _goldText.text = NumberFormatter.Format(gold);
        }
    }
}