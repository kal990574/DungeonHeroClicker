using _01.Scripts.Core.Utils;
using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade.Config
{
    [CreateAssetMenu(fileName = "CompanionUpgradeConfig", menuName = "DungeonHeroClicker/Config/Companion Upgrade")]
    public class CompanionUpgradeConfig : UpgradeConfigBase
    {
        [Header("Purchase")]
        [SerializeField] private long _purchaseCost;

        [Header("Visual")]
        [SerializeField] private GameObject _companionPrefab;
        [SerializeField] private Vector2 _spawnOffset;
        [SerializeField] private int _attackAnimIndex;

        [Header("Spawn Effect")]
        [SerializeField] private GameObject _spawnEffectPrefab;
        [SerializeField] private float _spawnEffectDuration = 1.5f;

        public BigNumber PurchaseCost => new BigNumber(_purchaseCost);
        public GameObject CompanionPrefab => _companionPrefab;
        public Vector2 SpawnOffset => _spawnOffset;
        public int AttackAnimIndex => _attackAnimIndex;
        public GameObject SpawnEffectPrefab => _spawnEffectPrefab;
        public float SpawnEffectDuration => _spawnEffectDuration;
    }
}