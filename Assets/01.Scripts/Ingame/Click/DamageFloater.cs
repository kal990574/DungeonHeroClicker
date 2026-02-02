using _01.Scripts.Core.Utils;
using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;

namespace _01.Scripts.Ingame.Click
{
    public class DamageFloater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshPro _damageText;

        [Header("Animation Settings")]
        [SerializeField] private float _floatHeight = 1f;
        [SerializeField] private float _duration = 0.8f;
        [SerializeField] private float _fadeStartTime = 0.5f;

        [Header("Random Offset")]
        [SerializeField] private float _randomOffsetX = 0.3f;

        private Vector3 _initialScale;
        private Sequence _sequence;

        private void Awake()
        {
            _initialScale = transform.localScale;
        }

        public void Show(BigNumber damage, Vector3 position)
        {
            // 위치 설정 (랜덤 X 오프셋).
            float randomX = Random.Range(-_randomOffsetX, _randomOffsetX);
            transform.position = position + new Vector3(randomX, 0f, 0f);

            // 텍스트 설정.
            _damageText.text = NumberFormatter.Format(damage);
            _damageText.alpha = 1f;
            transform.localScale = _initialScale;

            // 기존 시퀀스 정리.
            _sequence?.Kill();

            // DOTween 애니메이션.
            _sequence = DOTween.Sequence();

            // 위로 떠오르기.
            _sequence.Append(transform.DOMoveY(transform.position.y + _floatHeight, _duration)
                .SetEase(Ease.OutCubic));

            // 팝 효과.
            _sequence.Join(transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 1, 0.5f));

            // 페이드 아웃.
            _sequence.Insert(_fadeStartTime, _damageText.DOFade(0f, _duration - _fadeStartTime));

            // 완료 후 풀로 반환.
            _sequence.OnComplete(() => LeanPool.Despawn(gameObject));
        }

        private void OnDisable()
        {
            _sequence?.Kill();
        }
    }
}