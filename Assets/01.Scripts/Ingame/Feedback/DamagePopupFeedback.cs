using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public class DamagePopupFeedback : MonoBehaviour, IValueFeedback
    {
        [Header("Prefab")]
        [SerializeField] private GameObject _popupPrefab;

        [Header("Animation")]
        [SerializeField] private float _floatDistance = 1f;
        [SerializeField] private float _duration = 0.8f;
        [SerializeField] private Ease _moveEase = Ease.OutQuad;
        [SerializeField] private Ease _fadeEase = Ease.InQuad;

        [Header("Style")]
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _criticalColor = Color.yellow;
        [SerializeField] private float _criticalScale = 1.5f;

        public void Play(Vector3 position, float value)
        {
            if (_popupPrefab == null)
            {
                return;
            }

            var popup = Instantiate(_popupPrefab, position, Quaternion.identity);
            var text = popup.GetComponentInChildren<TMP_Text>();

            if (text == null)
            {
                Destroy(popup);
                return;
            }

            text.text = Mathf.RoundToInt(value).ToString();
            text.color = _normalColor;

            AnimatePopup(popup, text);
        }

        public void PlayCritical(Vector3 position, float value)
        {
            if (_popupPrefab == null)
            {
                return;
            }

            var popup = Instantiate(_popupPrefab, position, Quaternion.identity);
            popup.transform.localScale *= _criticalScale;

            var text = popup.GetComponentInChildren<TMP_Text>();

            if (text == null)
            {
                Destroy(popup);
                return;
            }

            text.text = Mathf.RoundToInt(value).ToString();
            text.color = _criticalColor;

            AnimatePopup(popup, text);
        }

        private void AnimatePopup(GameObject popup, TMP_Text text)
        {
            var targetPos = popup.transform.position + Vector3.up * _floatDistance;

            var sequence = DOTween.Sequence();
            sequence.Append(popup.transform.DOMove(targetPos, _duration).SetEase(_moveEase));
            sequence.Join(text.DOFade(0f, _duration).SetEase(_fadeEase));
            sequence.OnComplete(() => Destroy(popup));
        }

        public void Stop()
        {
            // 팝업은 자동 소멸
        }
    }
}