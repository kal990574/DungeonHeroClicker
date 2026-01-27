using UnityEngine;

namespace _01.Scripts.Ingame.Hero
{
    public class HeroAnimator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _attackAnimIndex;

        private SPUM_Prefabs _spumPrefabs;
        private bool _isInitialized;

        public void Initialize(SPUM_Prefabs spumPrefabs)
        {
            // 기존 초기화 상태 리셋
            _isInitialized = false;
            _spumPrefabs = spumPrefabs;
            InitializeAnimator();
        }

        private void InitializeAnimator()
        {
            if (_isInitialized || _spumPrefabs == null) return;

            if (_spumPrefabs._anim == null)
            {
                Debug.LogWarning("[HeroAnimator] SPUM_Prefabs._anim is null!");
                return;
            }

            if (_spumPrefabs._anim.runtimeAnimatorController == null)
            {
                Debug.LogWarning("[HeroAnimator] Animator has no controller!");
                return;
            }

            _spumPrefabs.OverrideControllerInit();
            _isInitialized = true;
        }

        public void PlayAttack()
        {
            if (_spumPrefabs == null || !_isInitialized) return;
            _spumPrefabs.PlayAnimation(PlayerState.ATTACK, _attackAnimIndex);
        }

        public void PlayIdle()
        {
            if (_spumPrefabs == null || !_isInitialized) return;
            _spumPrefabs.PlayAnimation(PlayerState.IDLE, 0);
        }
    }
}