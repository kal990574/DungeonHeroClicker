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

        [Header("Format")]
        [SerializeField] private string _format = "{0:N0}";

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
            _goldText.text = string.Format(_format, gold);
        }
    }
}