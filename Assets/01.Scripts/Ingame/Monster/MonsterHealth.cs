using System;
using UnityEngine;
using _01.Scripts.Core.Audio;
using _01.Scripts.Core.Utils;

namespace _01.Scripts.Ingame.Monster
{
    public class MonsterHealth : MonoBehaviour
    {
        private BigNumber _currentHealth;
        private BigNumber _maxHealth;

        public BigNumber CurrentHealth => _currentHealth;
        public BigNumber MaxHealth => _maxHealth;
        public bool IsDead => _currentHealth <= BigNumber.Zero;
        public double HealthRatio => _maxHealth > BigNumber.Zero ? _currentHealth.ToDouble() / _maxHealth.ToDouble() : 0.0;

        public event Action<BigNumber> OnDamaged;
        public event Action OnDeath;

        public void TakeDamage(BigNumber damage)
        {
            if (IsDead)
            {
                return;
            }

            _currentHealth -= damage;
            if (_currentHealth < BigNumber.Zero)
            {
                _currentHealth = BigNumber.Zero;
            }

            OnDamaged?.Invoke(damage);

            if (IsDead)
            {
                SFXManager.Instance?.PlayDeath();
                OnDeath?.Invoke();
            }
        }

        public void SetMaxHealth(BigNumber maxHealth)
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