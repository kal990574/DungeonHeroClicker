using _01.Scripts.Core.Utils;
using _01.Scripts.Outgame.Currency;
using _01.Scripts.UI.Effects;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _01.Scripts.UI
{
    public class GoldDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private TMP_Text _goldText;

        [Header("Effects")]
        [SerializeField] private UITextFeedback _textFeedback;
        [SerializeField] private FloatingTextSpawner _floatingTextSpawner;
        [SerializeField] private Color _floatingTextColor = new Color(1f, 0.85f, 0f);
        [SerializeField] private Vector2 _floatingTextOffset = new Vector2(-50f, 0f);

        [Header("Animation")]
        [SerializeField] private float _countDuration = 0.3f;
        [SerializeField] private Ease _countEase = Ease.OutQuad;

        [Header("Effect Accumulation")]
        [SerializeField] private float _effectCooldownDuration = 0.5f;

        private BigNumber _displayedGold;
        private BigNumber _previousGold;
        private Tweener _countTweener;

        private BigNumber _pendingGoldAmount;
        private float _effectCooldown;

        private void OnEnable()
        {
            _goldWallet.OnGoldChanged += HandleGoldChanged;
            _previousGold = _goldWallet.CurrentGold;
            _displayedGold = _previousGold;
            _goldText.text = NumberFormatter.Format(_displayedGold);
        }

        private void OnDisable()
        {
            _goldWallet.OnGoldChanged -= HandleGoldChanged;
        }

        private void Update()
        {
            if (_effectCooldown <= 0f)
            {
                return;
            }

            _effectCooldown -= Time.deltaTime;

            if (_effectCooldown <= 0f && _pendingGoldAmount > BigNumber.Zero)
            {
                ShowFloatingText(_pendingGoldAmount);
                _pendingGoldAmount = BigNumber.Zero;
            }
        }

        private void HandleGoldChanged(BigNumber targetGold)
        {
            BigNumber delta = targetGold - _previousGold;
            _previousGold = targetGold;

            UpdateDisplay(targetGold);

            if (delta > BigNumber.Zero)
            {
                AccumulateGold(delta);
            }
        }

        private void AccumulateGold(BigNumber amount)
        {
            _pendingGoldAmount += amount;

            if (_effectCooldown <= 0f)
            {
                if (_textFeedback != null)
                {
                    _textFeedback.Play();
                }

                _effectCooldown = _effectCooldownDuration;
            }
        }

        private void UpdateDisplay(BigNumber targetGold)
        {
            _countTweener?.Kill();

            // BigNumber는 DOTween 직접 지원 안하므로 double로 변환하여 애니메이션.
            double startValue = _displayedGold.ToDouble();
            double endValue = targetGold.ToDouble();

            _countTweener = DOTween.To(
                () => startValue,
                x =>
                {
                    _displayedGold = new BigNumber(x);
                    _goldText.text = NumberFormatter.Format(_displayedGold);
                },
                endValue,
                _countDuration
            ).SetEase(_countEase);
        }

        private void ShowFloatingText(BigNumber totalAmount)
        {
            if (_floatingTextSpawner == null)
            {
                return;
            }

            string text = $"+{NumberFormatter.Format(totalAmount)}";
            _floatingTextSpawner.SpawnAtUI(text, _goldText.rectTransform, _floatingTextColor, _floatingTextOffset);
        }

        private void OnDestroy()
        {
            _countTweener?.Kill();
        }
    }
}