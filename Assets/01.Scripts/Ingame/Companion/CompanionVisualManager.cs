using System.Collections.Generic;
using _01.Scripts.Core.Audio;
using _01.Scripts.Interfaces;
using _01.Scripts.Outgame.Upgrade;
using _01.Scripts.Outgame.Upgrade.Config;
using _01.Scripts.Outgame.Upgrade.Domain;
using UnityEngine;

namespace _01.Scripts.Ingame.Companion
{
    public class CompanionVisualManager : MonoBehaviour, ICompanionVisualManager
    {
        [Header("Dependencies")]
        [SerializeField] private UpgradeManager _upgradeManager;
        [SerializeField] private Transform _playerTransform;

        [Header("Companion Configs")]
        [SerializeField] private List<CompanionUpgradeConfig> _companionConfigs;

        private readonly Dictionary<string, CompanionVisualInstance> _visuals = new();

        private void Start()
        {
            RestoreAlreadyPurchased();
        }

        private void OnEnable()
        {
            _upgradeManager.OnItemPurchased += HandleItemPurchased;
        }

        private void OnDisable()
        {
            _upgradeManager.OnItemPurchased -= HandleItemPurchased;
        }

        private void HandleItemPurchased(UpgradeItem item)
        {
            if (item.Type == EUpgradeType.Companion)
            {
                SpawnCompanion(item.Id);
            }
        }

        private void RestoreAlreadyPurchased()
        {
            foreach (var config in _companionConfigs)
            {
                var item = _upgradeManager.GetItem(config.Id);
                if (item != null && item.IsPurchased)
                {
                    SpawnCompanion(item.Id);
                }
            }
        }

        public void SpawnCompanion(string companionId)
        {
            if (_visuals.ContainsKey(companionId))
            {
                return;
            }

            var config = _companionConfigs.Find(c => c.Id == companionId);
            if (config == null || config.CompanionPrefab == null)
            {
                Debug.LogWarning($"[CompanionVisualManager] Config not found for {companionId}");
                return;
            }

            Vector3 spawnPosition = CalculateSpawnPosition(config.SpawnOffset);

            PlaySpawnEffect(spawnPosition, config);
            var instance = CreateVisualInstance(spawnPosition, config);
            _visuals[companionId] = instance;

            Debug.Log($"[CompanionVisualManager] {config.DisplayName} spawned at {spawnPosition}");
        }

        public void PlayAllAttackAnimations()
        {
            foreach (var visual in _visuals.Values)
            {
                if (visual.Animator != null && visual.Animator.IsInitialized)
                {
                    visual.Animator.PlayAttack();
                }
            }
        }

        private Vector3 CalculateSpawnPosition(Vector2 offset)
        {
            Vector3 basePosition = _playerTransform != null
                ? _playerTransform.position
                : transform.position;

            return new Vector3(basePosition.x + offset.x, basePosition.y + offset.y, basePosition.z);
        }

        private void PlaySpawnEffect(Vector3 position, CompanionUpgradeConfig config)
        {
            SFXManager.Instance?.PlayTierUp();

            if (config.SpawnEffectPrefab == null)
            {
                return;
            }

            GameObject effect = Instantiate(config.SpawnEffectPrefab, position, Quaternion.identity);
            Destroy(effect, config.SpawnEffectDuration);
        }

        private CompanionVisualInstance CreateVisualInstance(Vector3 position, CompanionUpgradeConfig config)
        {
            var visualObject = Instantiate(config.CompanionPrefab, position, Quaternion.identity, transform);

            // 몬스터 방향(오른쪽)을 바라보도록 설정.
            visualObject.transform.localScale = new Vector3(-1, 1, 1);

            // Animator 초기화.
            CompanionAnimator animator = null;
            var spumPrefabs = visualObject.GetComponent<SPUM_Prefabs>();
            if (spumPrefabs != null)
            {
                animator = visualObject.AddComponent<CompanionAnimator>();
                animator.Initialize(spumPrefabs, config.AttackAnimIndex);
            }

            return new CompanionVisualInstance(visualObject, animator);
        }
    }

    // 내부 데이터 클래스: 스폰된 동료의 비주얼 인스턴스.
    internal class CompanionVisualInstance
    {
        public GameObject VisualObject { get; }
        public CompanionAnimator Animator { get; }

        public CompanionVisualInstance(GameObject visualObject, CompanionAnimator animator)
        {
            VisualObject = visualObject;
            Animator = animator;
        }
    }
}