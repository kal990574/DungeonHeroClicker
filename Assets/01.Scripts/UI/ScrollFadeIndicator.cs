using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts.UI
{
    public class ScrollFadeIndicator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private Image _bottomFade;

        [Header("Settings")]
        [SerializeField] private float _fadeThreshold = 0.02f;
        [SerializeField] private float _fadeSpeed = 5f;

        private float _bottomTargetAlpha;

        private void Start()
        {
            if (_scrollRect != null)
            {
                _scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
                UpdateFadeTargets(_scrollRect.normalizedPosition);
            }
        }

        private void OnDestroy()
        {
            if (_scrollRect != null)
            {
                _scrollRect.onValueChanged.RemoveListener(OnScrollValueChanged);
            }
        }

        private void Update()
        {
            UpdateFadeAlpha(_bottomFade, _bottomTargetAlpha);
        }

        private void OnScrollValueChanged(Vector2 normalizedPosition)
        {
            UpdateFadeTargets(normalizedPosition);
        }

        private void UpdateFadeTargets(Vector2 normalizedPosition)
        {
            float scrollY = normalizedPosition.y;

            // 맨 아래가 아니면 하단 페이드 표시
            _bottomTargetAlpha = scrollY > _fadeThreshold ? 1f : 0f;
        }

        private void UpdateFadeAlpha(Image fadeImage, float targetAlpha)
        {
            if (fadeImage == null)
            {
                return;
            }

            var color = fadeImage.color;
            color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime * _fadeSpeed);
            fadeImage.color = color;
        }
    }
}