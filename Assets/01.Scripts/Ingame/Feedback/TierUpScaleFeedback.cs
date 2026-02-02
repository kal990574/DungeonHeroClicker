using DG.Tweening;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public class TierUpScaleFeedback : MonoBehaviour, IFeedback
    {
        [Header("Target")]
        [SerializeField] private Transform _target;

        [Header("Settings")]
        [SerializeField] private float _expandScale = 1.3f;
        [SerializeField] private float _shrinkScale = 0.9f;
        [SerializeField] private float _expandDuration = 0.2f;
        [SerializeField] private float _shrinkDuration = 0.1f;
        [SerializeField] private float _restoreDuration = 0.15f;

        private Sequence _sequence;
        private Vector3 _originalScale;

        private void Awake()
        {
            if (_target == null)
            {
                _target = transform;
            }

            _originalScale = _target.localScale;
        }

        public void Play(Vector3 position)
        {
            Stop();

            _sequence = DOTween.Sequence();
            _sequence.Append(_target.DOScale(_originalScale * _expandScale, _expandDuration).SetEase(Ease.OutQuad));
            _sequence.Append(_target.DOScale(_originalScale * _shrinkScale, _shrinkDuration).SetEase(Ease.InQuad));
            _sequence.Append(_target.DOScale(_originalScale, _restoreDuration).SetEase(Ease.OutBack));
        }

        public void Stop()
        {
            _sequence?.Kill();
            if (_target != null)
            {
                _target.localScale = _originalScale;
            }
        }

        private void OnDestroy()
        {
            Stop();
        }
    }
}