using UnityEngine;

namespace _01.Scripts.Ingame.Hero
{
    public class CompanionAnimator : MonoBehaviour
    {
        private SPUM_Prefabs _spumPrefabs;
        private int _attackAnimIndex;
        private bool _isInitialized;

        public bool IsInitialized => _isInitialized;

        public void Initialize(SPUM_Prefabs spumPrefabs, int attackAnimIndex)
        {
            _isInitialized = false;
            _spumPrefabs = spumPrefabs;
            _attackAnimIndex = attackAnimIndex;
            InitializeAnimator();
        }

        private void InitializeAnimator()
        {
            if (_isInitialized || _spumPrefabs == null)
            {
                return;
            }

            if (_spumPrefabs._anim == null)
            {
                Debug.LogWarning("[CompanionAnimator] SPUM_Prefabs._anim is null!");
                return;
            }

            if (_spumPrefabs._anim.runtimeAnimatorController == null)
            {
                Debug.LogWarning("[CompanionAnimator] Animator has no controller!");
                return;
            }

            _spumPrefabs.OverrideControllerInit();
            _isInitialized = true;
        }

        public void PlayAttack()
        {
            if (_spumPrefabs == null || !_isInitialized)
            {
                return;
            }

            _spumPrefabs.PlayAnimation(PlayerState.ATTACK, _attackAnimIndex);
        }

        public void PlayIdle()
        {
            if (_spumPrefabs == null || !_isInitialized)
            {
                return;
            }

            _spumPrefabs.PlayAnimation(PlayerState.IDLE, 0);
        }
    }
}