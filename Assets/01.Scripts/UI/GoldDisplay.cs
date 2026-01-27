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

        private int _displayedGold;
        private int _previousGold;
        private Tweener _countTweener;

        private int _pendingGoldAmount;
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

            if (_effectCooldown <= 0f && _pendingGoldAmount > 0)
            {
                ShowFloatingText(_pendingGoldAmount);
                _pendingGoldAmount = 0;
            }
        }

        private void HandleGoldChanged(int targetGold)
        {
            int delta = targetGold - _previousGold;
            _previousGold = targetGold;

            UpdateDisplay(targetGold);

            // 골드 획득 시에만 효과 재생 (소비 시 제외).
            if (delta > 0)
            {
                AccumulateGold(delta);
            }
        }

        private void AccumulateGold(int amount)
        {
            _pendingGoldAmount += amount;

            // 첫 획득 시 즉시 피드백 재생, 타이머 시작.
            if (_effectCooldown <= 0f)
            {
                if (_textFeedback != null)
                {
                    _textFeedback.Play();
                }

                _effectCooldown = _effectCooldownDuration;
            }
        }

        private void UpdateDisplay(int targetGold)
        {
            _countTweener?.Kill();

            _countTweener = DOTween.To(
                () => _displayedGold,
                x =>
                {
                    _displayedGold = x;
                    _goldText.text = NumberFormatter.Format(_displayedGold);
                },
                targetGold,
                _countDuration
            ).SetEase(_countEase);
        }

        private void ShowFloatingText(int totalAmount)
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