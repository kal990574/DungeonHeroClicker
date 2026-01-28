using System;
using UnityEngine;
using _01.Scripts.Core.Audio;
namespace _01.Scripts.Ingame.Monster
{
    public class MonsterHealth : MonoBehaviour
    {
        [Header("Debug (Runtime Only)")]
        [SerializeField] private float _currentHealth;

        private float _maxHealth;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public bool IsDead => _currentHealth <= 0f;
        public float HealthRatio => _maxHealth > 0f ? _currentHealth / _maxHealth : 0f;

        public event Action<float> OnDamaged;
        public event Action OnDeath;

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