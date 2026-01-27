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

        [Header("Format")]
        [SerializeField] private string _format = "{0:F1}";

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
            _dpsText.text = string.Format(_format, _companionManager.TotalDPS);
        }
    }
}