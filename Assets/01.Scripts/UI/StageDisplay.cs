using _01.Scripts.Ingame.Stage;
using TMPro;
using UnityEngine;

namespace _01.Scripts.UI
{
    public class StageDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private StageManager _stageManager;
        [SerializeField] private TMP_Text _stageText;

        [Header("Format")]
        [SerializeField] private string _stageFormat = "{0} - {1}";

        private void OnEnable()
        {
            _stageManager.OnStageChanged += UpdateStageDisplay;
            _stageManager.OnKillCountChanged += UpdateStageDisplay;
        }

        private void OnDisable()
        {
            _stageManager.OnStageChanged -= UpdateStageDisplay;
            _stageManager.OnKillCountChanged -= UpdateStageDisplay;
        }

        private void UpdateStageDisplay(int stage)
        {
            UpdateDisplay();
        }

        private void UpdateStageDisplay(int current, int required)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (_stageText != null)
            {
                int stage = _stageManager.CurrentStage;
                int monsterNumber = _stageManager.CurrentKillCount + 1;
                _stageText.text = string.Format(_stageFormat, stage, monsterNumber);
            }
        }
    }
}