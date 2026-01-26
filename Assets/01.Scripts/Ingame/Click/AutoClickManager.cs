using UnityEngine;
using _01.Scripts.Interfaces;

namespace _01.Scripts.Ingame.Click
{
    public class AutoClickManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _tickInterval = 0.1f;

        private IDamageable _currentTarget;
        private Transform _targetTransform;
        private float _totalDPS;
        private float _damagePerTick;
        private float _timer;

        public float TotalDPS
        {
            get => _totalDPS;
            set
            {
                _totalDPS = value;
                _damagePerTick = _totalDPS * _tickInterval;
            }
        }

        public void SetTarget(IDamageable target, Transform targetTransform)
        {
            _currentTarget = target;
            _targetTransform = targetTransform;
        }

        public void ClearTarget()
        {
            _currentTarget = null;
            _targetTransform = null;
        }

        private void Update()
        {
            if (_currentTarget == null || _currentTarget.IsDead)
            {
                return;
            }

            if (_totalDPS <= 0f)
            {
                return;
            }

            _timer += Time.deltaTime;

            if (_timer >= _tickInterval)
            {
                _timer -= _tickInterval;
                ApplyAutoDamage();
            }
        }

        private void ApplyAutoDamage()
        {
            Vector3 targetPosition = _targetTransform != null
                ? _targetTransform.position
                : Vector3.zero;

            var clickInfo = new ClickInfo(
                damage: _damagePerTick,
                clickType: EClickType.Auto,
                position: targetPosition
            );

            _currentTarget.TakeDamage(clickInfo);
        }

        public void AddDPS(float amount)
        {
            TotalDPS += amount;
        }
    }
}