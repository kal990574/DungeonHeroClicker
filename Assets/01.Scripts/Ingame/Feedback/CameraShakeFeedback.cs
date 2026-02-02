using DG.Tweening;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    /// <summary>
    /// 크리티컬 히트 등 강한 타격 시 카메라를 흔드는 피드백.
    /// 씬에 배치하여 사용. 정적 메서드로 어디서든 호출 가능.
    /// </summary>
    public class CameraShakeFeedback : MonoBehaviour
    {
        public static CameraShakeFeedback Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private float _shakeDuration = 0.2f;
        [SerializeField] private float _shakeStrength = 0.3f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _randomness = 90f;

        private Camera _mainCamera;
        private Vector3 _originalPosition;
        private Tweener _tweener;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _mainCamera = Camera.main;

            if (_mainCamera != null)
            {
                _originalPosition = _mainCamera.transform.localPosition;
            }
        }

        public static void Shake()
        {
            Instance?.PlayShake();
        }

        private void PlayShake()
        {
            if (_mainCamera == null)
            {
                return;
            }

            Stop();

            _tweener = _mainCamera.transform
                .DOShakePosition(_shakeDuration, _shakeStrength, _vibrato, _randomness)
                .SetEase(Ease.OutQuad)
                .OnComplete(RestorePosition);
        }

        public void Stop()
        {
            _tweener?.Kill();
            RestorePosition();
        }

        private void RestorePosition()
        {
            if (_mainCamera != null)
            {
                _mainCamera.transform.localPosition = _originalPosition;
            }
        }

        private void OnDestroy()
        {
            Stop();

            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}