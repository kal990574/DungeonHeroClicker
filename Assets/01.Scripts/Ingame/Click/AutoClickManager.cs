using _01.Scripts.Ingame.Hero;
using _01.Scripts.Interfaces;
using UnityEngine;

namespace _01.Scripts.Ingame.Click
{
    public class AutoClickManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _tickInterval = 1f;

        [Header("Critical")]
        [SerializeField] private float _criticalChance = 0.5f;
        [SerializeField] private float _criticalMultiplier = 2f;

        [Header("Dependencies")]
        [SerializeField] private CompanionManager _companionManager;

        private IDamageable _currentTarget;
        private Transform _targetTransform;
        private float _timer;

        private float TotalDPS => _companionManager != null ? _companionManager.TotalDPS : 0f;

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

            if (TotalDPS <= 0f)
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

            bool isCritical = Random.value < _criticalChance;
            float baseDamage = TotalDPS * _tickInterval;
            float damage = isCritical ? baseDamage * _criticalMultiplier : baseDamage;

            var clickInfo = new ClickInfo(
                damage: damage,
                clickType: EClickType.Auto,
                position: targetPosition,
                isCritical: isCritical
            );

            _currentTarget.TakeDamage(clickInfo);
        }
    }
}