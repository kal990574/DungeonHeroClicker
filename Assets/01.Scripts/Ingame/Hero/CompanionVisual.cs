using UnityEngine;

namespace _01.Scripts.Ingame.Hero
{
    public class CompanionVisual : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Companion _companion;
        [SerializeField] private Transform _playerTransform;

        private GameObject _visualInstance;
        private CompanionAnimator _animator;
        private bool _isSpawned;

        public CompanionAnimator Animator => _animator;
        public bool IsSpawned => _isSpawned;

        private void OnEnable()
        {
            if (_companion != null)
            {
                _companion.OnPurchased += HandleCompanionPurchased;
            }
        }

        private void OnDisable()
        {
            if (_companion != null)
            {
                _companion.OnPurchased -= HandleCompanionPurchased;
            }
        }

        private void HandleCompanionPurchased(Companion companion)
        {
            SpawnCompanion();
        }

        public void SpawnCompanion()
        {
            if (_isSpawned)
            {
                return;
            }

            var data = _companion.Data;
            if (data.CompanionPrefab == null)
            {
                Debug.LogWarning($"[CompanionVisual] {data.CompanionName} has no prefab assigned!");
                return;
            }

            Vector3 spawnPosition = CalculateSpawnPosition();

            PlaySpawnEffect(spawnPosition, data);
            CreateVisualInstance(spawnPosition, data);

            _isSpawned = true;
            Debug.Log($"[CompanionVisual] {data.CompanionName} spawned at {spawnPosition}");
        }

        private Vector3 CalculateSpawnPosition()
        {
            Vector3 basePosition = _playerTransform != null
                ? _playerTransform.position
                : transform.position;

            Vector2 offset = _companion.Data.SpawnOffset;
            return new Vector3(basePosition.x + offset.x, basePosition.y + offset.y, basePosition.z);
        }

        private void PlaySpawnEffect(Vector3 position, CompanionData data)
        {
            if (data.SpawnEffectPrefab == null)
            {
                return;
            }

            GameObject effect = Instantiate(data.SpawnEffectPrefab, position, Quaternion.identity);
            Destroy(effect, data.SpawnEffectDuration);
        }

        private void CreateVisualInstance(Vector3 position, CompanionData data)
        {
            _visualInstance = Instantiate(data.CompanionPrefab, position, Quaternion.identity, transform);

            // 몬스터 방향(오른쪽)을 바라보도록 설정.
            _visualInstance.transform.localScale = new Vector3(-1, 1, 1);

            // Animator 초기화.
            var spumPrefabs = _visualInstance.GetComponent<SPUM_Prefabs>();
            if (spumPrefabs != null)
            {
                _animator = gameObject.AddComponent<CompanionAnimator>();
                _animator.Initialize(spumPrefabs, data.AttackAnimIndex);
            }
        }

        public void PlayAttackAnimation()
        {
            if (_animator != null && _animator.IsInitialized)
            {
                _animator.PlayAttack();
            }
        }
    }
}