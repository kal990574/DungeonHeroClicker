using _01.Scripts.Core.Utils;
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
            if (_monsterHealth == null)
            {
                return;
            }

            _monsterHealth.OnDamaged += HandleDamaged;
            UpdateBar();
        }

        private void OnDisable()
        {
            if (_monsterHealth == null)
            {
                return;
            }

            _monsterHealth.OnDamaged -= HandleDamaged;
        }

        private void HandleDamaged(BigNumber damage)
        {
            UpdateBar();
        }

        private void UpdateBar()
        {
            if (_monsterHealth == null)
            {
                return;
            }

            _fillImage.fillAmount = (float)_monsterHealth.HealthRatio;
        }

        public void SetTarget(MonsterHealth target)
        {
            if (_monsterHealth != null)
            {
                _monsterHealth.OnDamaged -= HandleDamaged;
            }

            _monsterHealth = target;

            if (_monsterHealth != null)
            {
                _monsterHealth.OnDamaged += HandleDamaged;
                UpdateBar();
            }
        }
    }
}