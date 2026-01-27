using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public class TierUpTextFeedback : MonoBehaviour, IFeedback
    {
        [Header("Prefab")]
        [SerializeField] private GameObject _textPrefab;

        [Header("Animation")]
        [SerializeField] private float _floatDistance = 3f;
        [SerializeField] private float _duration = 1.5f;
        [SerializeField] private float _scaleMultiplier = 1.5f;
        [SerializeField] private Ease _moveEase = Ease.OutQuad;
        [SerializeField] private Ease _fadeEase = Ease.InQuad;

        [Header("Style")]
        [SerializeField] private Color _textColor = new Color(1f, 0.84f, 0f);

        private string _tierName;
        private Vector3 _defaultScale;

        private void Awake()
        {
            if (_textPrefab != null)
            {
                _defaultScale = _textPrefab.transform.localScale;
            }
        }

        public void SetTierName(string tierName)
        {
            _tierName = tierName;
        }

        public void Play(Vector3 position)
        {
            if (_textPrefab == null || string.IsNullOrEmpty(_tierName)) return;

            var popup = LeanPool.Spawn(_textPrefab, position, Quaternion.identity);
            popup.transform.localScale = _defaultScale * _scaleMultiplier;

            var text = popup.GetComponentInChildren<TMP_Text>();
            if (text == null)
            {
                LeanPool.Despawn(popup);
                return;
            }

            text.text = $"{_tierName}!";
            text.color = _textColor;
            text.alpha = 1f;

            AnimatePopup(popup, text);
        }

        public void Stop()
        {
            // 풀링으로 관리되므로 별도 처리 불필요.
        }

        private void AnimatePopup(GameObject popup, TMP_Text text)
        {
            var targetPos = popup.transform.position + Vector3.up * _floatDistance;

            var sequence = DOTween.Sequence();
            sequence.Append(popup.transform.DOMove(targetPos, _duration).SetEase(_moveEase));
            sequence.Join(text.DOFade(0f, _duration).SetEase(_fadeEase).SetDelay(_duration * 0.5f));
            sequence.OnComplete(() => LeanPool.Despawn(popup));
        }
    }
}