using System;
using _01.Scripts.Ingame.Click;
using _01.Scripts.Ingame.Feedback;
using _01.Scripts.Interfaces;
using UnityEngine;

namespace _01.Scripts.Ingame.Monster
{
    public class Monster : MonoBehaviour, IDamageable, IClickable
    {
        [Header("Components")]
        [SerializeField] private MonsterHealth _health;
        [SerializeField] private ClickTarget _clickTarget;
        [SerializeField] private FeedbackPlayer _feedbackPlayer;
        [SerializeField] private DeathParticleFeedback _deathParticleFeedback;

        [Header("Stat Multipliers")]
        [SerializeField] private float _healthMultiplier = 1f;
        [SerializeField] private float _goldMultiplier = 1f;

        public float HealthMultiplier => _healthMultiplier;
        public float GoldMultiplier => _goldMultiplier;
        public float CurrentHealth => _health.CurrentHealth;
        public float MaxHealth => _health.MaxHealth;
        public bool IsDead => _health.IsDead;
        public bool IsClickable => !IsDead;

        public event Action OnMonsterDeath;

        private void Awake()
        {
            if (_health == null)
            {
                _health = GetComponent<MonsterHealth>();
            }

            if (_clickTarget == null)
            {
                _clickTarget = GetComponent<ClickTarget>();
            }

            if (_feedbackPlayer == null)
            {
                _feedbackPlayer = GetComponent<FeedbackPlayer>();
            }
        }

        private void OnEnable()
        {
            _health.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            _health.OnDeath -= HandleDeath;
        }

        public void TakeDamage(ClickInfo clickInfo)
        {
            if (IsDead)
            {
                return;
            }

            _health.TakeDamage(clickInfo.Damage);
            _feedbackPlayer?.PlayWithDamage(clickInfo.Position, clickInfo.Damage, clickInfo.IsCritical);

            Debug.Log($"[{clickInfo.ClickType}] Damage: {clickInfo.Damage}, HP: {CurrentHealth}/{MaxHealth}");
        }

        public void OnClick()
        {
        }

        private void HandleDeath()
        {
            _clickTarget?.SetClickable(false);
            _deathParticleFeedback?.Play(transform.position);
            OnMonsterDeath?.Invoke();
            gameObject.SetActive(false);
            Debug.Log("Monster Dead!");
        }

        public void Reset()
        {
            _health.Reset();
            _clickTarget?.SetClickable(true);
            gameObject.SetActive(true);
        }
    }
}