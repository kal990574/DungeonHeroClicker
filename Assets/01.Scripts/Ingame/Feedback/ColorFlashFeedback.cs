using DG.Tweening;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    /// <summary>
    /// 피격 시 스프라이트를 흰색으로 플래시하는 피드백.
    /// </summary>
    public class ColorFlashFeedback : MonoBehaviour, IFeedback
    {
        [Header("Target")]
        [SerializeField] private Transform _targetRoot;

        [Header("Settings")]
        [SerializeField] private Color _flashColor = Color.white;
        [SerializeField] private float _flashDuration = 0.1f;

        private SpriteRenderer[] _renderers;
        private Color[] _originalColors;
        private Sequence _sequence;

        private void Awake()
        {
            if (_targetRoot == null)
            {
                _targetRoot = transform;
            }

            CacheRenderers();
        }

        private void CacheRenderers()
        {
            _renderers = _targetRoot.GetComponentsInChildren<SpriteRenderer>();
            _originalColors = new Color[_renderers.Length];

            for (int i = 0; i < _renderers.Length; i++)
            {
                _originalColors[i] = _renderers[i].color;
            }
        }

        public void Play(Vector3 position)
        {
            Stop();

            _sequence = DOTween.Sequence();

            for (int i = 0; i < _renderers.Length; i++)
            {
                var renderer = _renderers[i];
                var originalColor = _originalColors[i];

                // 플래시 색상으로 전환 후 원래 색상으로 복귀.
                _sequence.Join(
                    renderer.DOColor(_flashColor, _flashDuration * 0.5f)
                        .SetEase(Ease.OutQuad)
                );
            }

            _sequence.AppendCallback(() =>
            {
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].DOColor(_originalColors[i], _flashDuration * 0.5f)
                        .SetEase(Ease.InQuad);
                }
            });
        }

        public void Stop()
        {
            _sequence?.Kill();

            // 원래 색상으로 즉시 복원.
            for (int i = 0; i < _renderers.Length; i++)
            {
                if (_renderers[i] != null)
                {
                    _renderers[i].color = _originalColors[i];
                }
            }
        }

        private void OnDestroy()
        {
            Stop();
        }
    }
}