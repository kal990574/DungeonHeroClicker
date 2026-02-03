using _01.Scripts.Core.Utils;
using _01.Scripts.Outgame.Upgrade;
using _01.Scripts.UI.Effects;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _01.Scripts.UI
{
    public class DPSDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UpgradeManager _upgradeManager;
        [SerializeField] private TMP_Text _dpsText;

        [Header("Effects")]
        [SerializeField] private UITextFeedback _textFeedback;
        [SerializeField] private FloatingTextSpawner _floatingTextSpawner;
        [SerializeField] private Color _floatingTextColor = new Color(0.4f, 1f, 0.4f);

        [Header("Animation")]
        [SerializeField] private float _countDuration = 0.3f;
        [SerializeField] private Ease _countEase = Ease.OutQuad;

        private BigNumber _displayedDPS;
        private BigNumber _previousDPS;
        private Tweener _countTweener;

        private void OnEnable()
        {
            _upgradeManager.OnTotalDPSChanged += HandleDPSChanged;
        }

        private void Start()
        {
            _previousDPS = _upgradeManager.TotalDPS;
            _displayedDPS = _previousDPS;
            _dpsText.text = NumberFormatter.Format(_displayedDPS);
        }

        private void OnDisable()
        {
            _upgradeManager.OnTotalDPSChanged -= HandleDPSChanged;
        }

        private void HandleDPSChanged()
        {
            BigNumber targetDPS = _upgradeManager.TotalDPS;
            BigNumber delta = targetDPS - _previousDPS;
            _previousDPS = targetDPS;

            UpdateDisplay(targetDPS);

            // DPS 증가 시에만 효과 재생.
            if (delta > BigNumber.Zero)
            {
                PlayEffects(delta);
            }
        }

        private void UpdateDisplay(BigNumber targetDPS)
        {
            _countTweener?.Kill();

            double startValue = _displayedDPS.ToDouble();
            double endValue = targetDPS.ToDouble();

            _countTweener = DOTween.To(
                () => startValue,
                x =>
                {
                    _displayedDPS = new BigNumber(x);
                    _dpsText.text = NumberFormatter.Format(_displayedDPS);
                },
                endValue,
                _countDuration
            ).SetEase(_countEase);
        }

        private void PlayEffects(BigNumber gainedAmount)
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