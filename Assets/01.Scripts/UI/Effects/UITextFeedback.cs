using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _01.Scripts.UI.Effects
{
    public class UITextFeedback : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform _scaleTarget;
        [SerializeField] private TMP_Text _colorTarget;

        [Header("Scale Punch")]
        [SerializeField] private float _punchScale = 0.2f;
        [SerializeField] private float _punchDuration = 0.3f;
        [SerializeField] private int _vibrato = 5;
        [SerializeField] private float _elasticity = 0.5f;

        [Header("Color Flash")]
        [SerializeField] private Color _flashColor = Color.yellow;
        [SerializeField] private float _flashDuration = 0.3f;

        private Vector3 _originalScale;
        private Color _originalColor;
        private Tweener _scaleTweener;
        private Tweener _colorTweener;

        private void Awake()
        {
            if (_scaleTarget != null)
            {
                _originalScale = _scaleTarget.localScale;
            }

            if (_colorTarget != null)
            {
                _originalColor = _colorTarget.color;
            }
        }

        public void Play()
        {
            PlayScalePunch();
            PlayColorFlash();
        }

        public void PlayScalePunch()
        {
            if (_scaleTarget == null)
            {
                return;
            }

            _scaleTweener?.Kill(true);
            _scaleTarget.localScale = _originalScale;

            _scaleTweener = _scaleTarget
                .DOPunchScale(Vector3.one * _punchScale, _punchDuration, _vibrato, _elasticity)
                .SetEase(Ease.OutQuad);
        }

        public void PlayColorFlash()
        {
            if (_colorTarget == null)
            {
                return;
            }

            _colorTweener?.Kill(true);
            _colorTarget.color = _flashColor;

            _colorTweener = _colorTarget
                .DOColor(_originalColor, _flashDuration)
                .SetEase(Ease.OutQuad);
        }

        private void OnDestroy()
        {
            _scaleTweener?.Kill();
            _colorTweener?.Kill();
        }
    }
}