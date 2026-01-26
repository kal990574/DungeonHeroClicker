using _01.Scripts.Ingame.Monster;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Scripts.UI
{
    public class MonsterHealthBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MonsterHealth _monsterHealth;
        [SerializeField] private Image _fillImage;

        private void OnEnable()
        {
            _monsterHealth.OnDamaged += HandleDamaged;
            UpdateBar();
        }

        private void OnDisable()
        {
            _monsterHealth.OnDamaged -= HandleDamaged;
        }

        private void HandleDamaged(float damage)
        {
            UpdateBar();
        }

        private void UpdateBar()
        {
            _fillImage.fillAmount = _monsterHealth.CurrentHealth / _monsterHealth.MaxHealth;
        }

        public void SetTarget(MonsterHealth target)
        {
            if (_monsterHealth != null)
            {
                _monsterHealth.OnDamaged -= HandleDamaged;
            }

            _monsterHealth = target;
            _monsterHealth.OnDamaged += HandleDamaged;
            UpdateBar();
        }
    }
}