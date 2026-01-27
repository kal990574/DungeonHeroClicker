using _01.Scripts.Core.Utils;
using _01.Scripts.Ingame.Hero;
using TMPro;
using UnityEngine;

namespace _01.Scripts.UI
{
    public class DPSDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CompanionManager _companionManager;
        [SerializeField] private TMP_Text _dpsText;

        private void OnEnable()
        {
            _companionManager.OnDPSChanged += UpdateDisplay;
            UpdateDisplay();
        }

        private void OnDisable()
        {
            _companionManager.OnDPSChanged -= UpdateDisplay;
        }

        private void UpdateDisplay()
        {
            _dpsText.text = NumberFormatter.Format(_companionManager.TotalDPS);
        }
    }
}