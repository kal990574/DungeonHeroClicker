using _01.Scripts.Core.Utils;
using _01.Scripts.Outgame.Currency;
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

        [Header("Animation")]
        [SerializeField] private float _countDuration = 0.3f;
        [SerializeField] private Ease _countEase = Ease.OutQuad;

        private int _displayedGold;
        private Tweener _countTweener;

        private void OnEnable()
        {
            _goldWallet.OnGoldChanged += UpdateDisplay;
            UpdateDisplay(_goldWallet.CurrentGold);
        }

        private void OnDisable()
        {
            _goldWallet.OnGoldChanged -= UpdateDisplay;
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

        private void OnDestroy()
        {
            _countTweener?.Kill();
        }
    }
}