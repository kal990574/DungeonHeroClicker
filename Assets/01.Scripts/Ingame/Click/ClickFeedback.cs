using DG.Tweening;
using UnityEngine;

namespace _01.Scripts.Ingame.Click
{
    public class ClickFeedback : MonoBehaviour
    {
        [Header("Visual")]
        [SerializeField] private GameObject _hitEffectPrefab;

        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _hitSound;

        [Header("Scale Punch")]
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private float _punchScale = 0.2f;
        [SerializeField] private float _punchDuration = 0.15f;
        [SerializeField] private int _punchVibrato = 1;
        [SerializeField] private float _punchElasticity = 0.5f;

        private Tweener _scaleTween;

        public void PlayFeedback(Vector3 position)
        {
            PlayHitEffect(position);
            PlayHitSound();
            PlayScalePunch();
        }

        private void PlayHitEffect(Vector3 position)
        {
            if (_hitEffectPrefab == null)
            {
                return;
            }

            Instantiate(_hitEffectPrefab, position, Quaternion.identity);
        }

        private void PlayHitSound()
        {
            if (_audioSource == null || _hitSound == null)
            {
                return;
            }

            _audioSource.PlayOneShot(_hitSound);
        }

        private void PlayScalePunch()
        {
            if (_targetTransform == null)
            {
                return;
            }

            _scaleTween?.Kill(true);
            _scaleTween = _targetTransform
                .DOPunchScale(Vector3.one * _punchScale, _punchDuration, _punchVibrato, _punchElasticity)
                .SetEase(Ease.OutQuad);
        }

        private void OnDestroy()
        {
            _scaleTween?.Kill();
        }
    }
}