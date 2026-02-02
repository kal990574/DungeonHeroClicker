using _01.Scripts.Core.Utils;
using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public class DamagePopupFeedback : MonoBehaviour, IValueFeedback
    {
        [Header("Prefab")]
        [SerializeField] private GameObject _popupPrefab;

        [Header("Animation")]
        [SerializeField] private float _floatDistance = 2f;
        [SerializeField] private float _duration = 0.8f;
        [SerializeField] private Ease _moveEase = Ease.OutQuad;
        [SerializeField] private Ease _fadeEase = Ease.InQuad;

        [Header("Random Offset")]
        [SerializeField] private float _randomOffsetX = 0.3f;

        [Header("Style")]
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _criticalColor = Color.yellow;
        [SerializeField] private float _criticalScale = 1.5f;

        private Vector3 _defaultScale;

        private void Awake()
        {
            if (_popupPrefab != null)
            {
                _defaultScale = _popupPrefab.transform.localScale;
            }
        }

        public void Play(Vector3 position, BigNumber value)
        {
            if (_popupPrefab == null) return;

            // 랜덤 X 오프셋 적용.
            float randomX = Random.Range(-_randomOffsetX, _randomOffsetX);
            var spawnPos = position + new Vector3(randomX, 0f, 0f);

            var popup = LeanPool.Spawn(_popupPrefab, spawnPos, Quaternion.identity);
            popup.transform.localScale = _defaultScale;

            var text = popup.GetComponentInChildren<TMP_Text>();
            if (text == null)
            {
                LeanPool.Despawn(popup);
                return;
            }

            text.text = NumberFormatter.Format(value);
            text.color = _normalColor;
            text.alpha = 1f;

            AnimatePopup(popup, text);
        }

        public void PlayCritical(Vector3 position, BigNumber value)
        {
            if (_popupPrefab == null) return;

            // 랜덤 X 오프셋 적용.
            float randomX = Random.Range(-_randomOffsetX, _randomOffsetX);
            var spawnPos = position + new Vector3(randomX, 0f, 0f);

            var popup = LeanPool.Spawn(_popupPrefab, spawnPos, Quaternion.identity);
            popup.transform.localScale = _defaultScale * _criticalScale;

            var text = popup.GetComponentInChildren<TMP_Text>();
            if (text == null)
            {
                LeanPool.Despawn(popup);
                return;
            }

            text.text = NumberFormatter.Format(value);
            text.color = _criticalColor;
            text.alpha = 1f;

            AnimatePopup(popup, text);
        }

        private void AnimatePopup(GameObject popup, TMP_Text text)
        {
            var targetPos = popup.transform.position + Vector3.up * _floatDistance;

            var sequence = DOTween.Sequence();
            sequence.Append(popup.transform.DOMove(targetPos, _duration).SetEase(_moveEase));
            sequence.Join(text.DOFade(0f, _duration).SetEase(_fadeEase));
            sequence.OnComplete(() => LeanPool.Despawn(popup));
        }

        public void Stop()
        {
            // 풀링으로 관리되므로 별도 처리 불필요.
        }
    }
}