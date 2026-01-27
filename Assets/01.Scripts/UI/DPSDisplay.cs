using _01.Scripts.Core.Utils;
using _01.Scripts.Ingame.Hero;
using _01.Scripts.UI.Effects;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _01.Scripts.UI
{
    public class DPSDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CompanionManager _companionManager;
        [SerializeField] private TMP_Text _dpsText;

        [Header("Effects")]
        [SerializeField] private UITextFeedback _textFeedback;
        [SerializeField] private FloatingTextSpawner _floatingTextSpawner;
        [SerializeField] private Color _floatingTextColor = new Color(0.4f, 1f, 0.4f);

        [Header("Animation")]
        [SerializeField] private float _countDuration = 0.3f;
        [SerializeField] private Ease _countEase = Ease.OutQuad;

        private float _displayedDPS;
        private float _previousDPS;
        private Tweener _countTweener;

        private void OnEnable()
        {
            _companionManager.OnDPSChanged += HandleDPSChanged;
            _previousDPS = _companionManager.TotalDPS;
            _displayedDPS = _previousDPS;
            _dpsText.text = NumberFormatter.Format(_displayedDPS);
        }

        private void OnDisable()
        {
            _companionManager.OnDPSChanged -= HandleDPSChanged;
        }

        private void HandleDPSChanged()
        {
            float targetDPS = _companionManager.TotalDPS;
            float delta = targetDPS - _previousDPS;
            _previousDPS = targetDPS;

            UpdateDisplay(targetDPS);

            // DPS 증가 시에만 효과 재생.
            if (delta > 0)
            {
                PlayEffects(delta);
            }
        }

        private void UpdateDisplay(float targetDPS)
        {
            _countTweener?.Kill();

            _countTweener = DOTween.To(
                () => _displayedDPS,
                x =>
                {
                    _displayedDPS = x;
                    _dpsText.text = NumberFormatter.Format(_displayedDPS);
                },
                targetDPS,
                _countDuration
            ).SetEase(_countEase);
        }

        private void PlayEffects(float gainedAmount)
        {
            if (_textFeedback != null)
            {
                _textFeedback.Play();
            }

            if (_floatingTextSpawner != null)
            {
                string text = $"+{NumberFormatter.Format(gainedAmount)}";
                _floatingTextSpawner.SpawnAtUI(text, _dpsText.rectTransform, _floatingTextColor);
            }
        }

        private void OnDestroy()
        {
            _countTweener?.Kill();
        }
    }
}