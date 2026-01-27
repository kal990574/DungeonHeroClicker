using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts.UI.Effects
{
    /// <summary>
    /// 버튼 클릭 시 스케일 펀치 효과를 재생하는 피드백.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ButtonFeedback : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _punchScale = 0.1f;
        [SerializeField] private float _duration = 0.15f;
        [SerializeField] private int _vibrato = 1;
        [SerializeField] private float _elasticity = 0.5f;

        private Button _button;
        private Vector3 _originalScale;
        private Tweener _tweener;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _originalScale = transform.localScale;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(PlayFeedback);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(PlayFeedback);
        }

        private void PlayFeedback()
        {
            _tweener?.Kill(true);
            transform.localScale = _originalScale;

            _tweener = transform
                .DOPunchScale(Vector3.one * _punchScale, _duration, _vibrato, _elasticity)
                .SetEase(Ease.OutQuad);
        }

        private void OnDestroy()
        {
            _tweener?.Kill();
        }
    }
}