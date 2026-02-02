using _01.Scripts.Core.Utils;
using UnityEngine;

namespace _01.Scripts.Ingame.Monster
{
    public class MonsterAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SPUM_Prefabs _spumPrefabs;
        [SerializeField] private MonsterHealth _health;

        [Header("Settings")]
        [SerializeField] private int _damagedAnimIndex;
        [SerializeField] private int _deathAnimIndex;

        private bool _isInitialized;

        private void Start()
        {
            if (_spumPrefabs == null)
            {
                _spumPrefabs = GetComponentInChildren<SPUM_Prefabs>();
            }

            if (_health == null)
            {
                _health = GetComponent<MonsterHealth>();
            }

            InitializeAnimator();
        }

        private void OnEnable()
        {
            if (_health != null)
            {
                _health.OnDamaged += HandleDamaged;
                _health.OnDeath += PlayDeath;
            }
        }

        private void OnDisable()
        {
            if (_health != null)
            {
                _health.OnDamaged -= HandleDamaged;
                _health.OnDeath -= PlayDeath;
            }
        }

        private void InitializeAnimator()
        {
            if (_isInitialized || _spumPrefabs == null) return;

            if (_spumPrefabs._anim == null)
            {
                Debug.LogWarning("[MonsterAnimator] SPUM_Prefabs._anim is null!");
                return;
            }

            if (_spumPrefabs._anim.runtimeAnimatorController == null)
            {
                Debug.LogWarning("[MonsterAnimator] Animator has no controller!");
                return;
            }

            _spumPrefabs.OverrideControllerInit();
            _isInitialized = true;
        }

        private void HandleDamaged(BigNumber damage)
        {
            PlayDamaged();
        }

        public void PlayDamaged()
        {
            if (!_isInitialized) InitializeAnimator();
            if (_spumPrefabs == null || !_isInitialized) return;
            _spumPrefabs.PlayAnimation(PlayerState.DAMAGED, _damagedAnimIndex);
        }

        public void PlayDeath()
        {
            if (!_isInitialized) InitializeAnimator();
            if (_spumPrefabs == null || !_isInitialized) return;
            _spumPrefabs.PlayAnimation(PlayerState.DEATH, _deathAnimIndex);
        }
    }
}