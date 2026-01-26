using _01.Scripts.Ingame.Click;
using _01.Scripts.Interfaces;
using UnityEngine;

namespace _01.Scripts.Ingame.Monster
{
    public class TestMonster : MonoBehaviour, IDamageable, IClickable
    {
        [Header("Stats")]
        [SerializeField] private float _maxHealth = 100f;

        private float _currentHealth;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public bool IsDead => _currentHealth <= 0f;
        public bool IsClickable => !IsDead;

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(ClickInfo clickInfo)
        {
            if (IsDead)
            {
                return;
            }

            _currentHealth -= clickInfo.Damage;
            Debug.Log($"[{clickInfo.ClickType}] Damage: {clickInfo.Damage}, HP: {_currentHealth}/{_maxHealth}");

            if (IsDead)
            {
                OnDeath();
            }
        }

        public void OnClick()
        {
            Debug.Log("Monster Clicked!");
        }

        private void OnDeath()
        {
            Debug.Log("Monster Dead!");
            gameObject.SetActive(false);
        }
    }
}