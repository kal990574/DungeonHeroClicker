using System;
using UnityEngine;
using _01.Scripts.Core.Audio;
namespace _01.Scripts.Ingame.Monster
{
    public class MonsterHealth : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private float _maxHealth = 100f;

        private float _currentHealth;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public bool IsDead => _currentHealth <= 0f;
        public float HealthRatio => _currentHealth / _maxHealth;

        public event Action<float> OnDamaged;
        public event Action OnDeath;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (IsDead)
            {
                return;
            }
            
            _currentHealth -= damage;
            _currentHealth = Mathf.Max(0f, _currentHealth);
            
            OnDamaged?.Invoke(damage);

            if (IsDead)
            {
                SFXManager.Instance?.PlayDeath();
                OnDeath?.Invoke();
            }
        }
        
        public void SetMaxHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
        }

        public void Reset()
        {
            _currentHealth = _maxHealth;
        }
    }
}