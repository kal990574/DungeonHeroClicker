using DG.Tweening;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public class ScaleFeedback : MonoBehaviour, IFeedback
    {
        [Header("Settings")]
        [SerializeField] private Transform _target;
        [SerializeField] private float _punchScale = 0.2f;
        [SerializeField] private float _duration = 0.15f;
        [SerializeField] private int _vibrato = 1;
        [SerializeField] private float _elasticity = 0.5f;

        private Tweener _tweener;

        private void Awake()
        {
            if (_target == null)
            {
                _target = transform;
            }
        }

        public void Play(Vector3 position)
        {
            Stop();
            _tweener = _target
                .DOPunchScale(Vector3.one * _punchScale, _duration, _vibrato, _elasticity)
                .SetEase(Ease.OutQuad);
        }

        public void Stop()
        {
            _tweener?.Kill(true);
        }

        private void OnDestroy()
        {
            Stop();
        }
    }
}