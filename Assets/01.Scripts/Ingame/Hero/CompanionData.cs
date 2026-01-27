using UnityEngine;

namespace _01.Scripts.Ingame.Hero
{
    [CreateAssetMenu(fileName = "CompanionData", menuName = "DungeonHeroClicker/Companion Data")]
    public class CompanionData : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private string _companionName;
        [SerializeField] private Sprite _icon;

        [Header("Purchase")]
        [SerializeField] private int _purchaseCost;

        [Header("DPS")]
        [SerializeField] private float _baseDPS = 1f;
        [SerializeField] private float _dpsMultiplier = 1.12f;

        [Header("Upgrade Cost")]
        [SerializeField] private int _baseUpgradeCost = 100;
        [SerializeField] private float _costMultiplier = 1.15f;

        [Header("Visual")]
        [SerializeField] private GameObject _companionPrefab;
        [SerializeField] private Vector2 _spawnOffset;
        [SerializeField] private int _attackAnimIndex;

        [Header("Spawn Effect")]
        [SerializeField] private GameObject _spawnEffectPrefab;
        [SerializeField] private float _spawnEffectDuration = 1.5f;

        public string CompanionName => _companionName;
        public Sprite Icon => _icon;
        public int PurchaseCost => _purchaseCost;
        public float BaseDPS => _baseDPS;
        public float DPSMultiplier => _dpsMultiplier;
        public GameObject CompanionPrefab => _companionPrefab;
        public Vector2 SpawnOffset => _spawnOffset;
        public int AttackAnimIndex => _attackAnimIndex;
        public GameObject SpawnEffectPrefab => _spawnEffectPrefab;
        public float SpawnEffectDuration => _spawnEffectDuration;

        public int GetUpgradeCost(int level)
        {
            return Mathf.RoundToInt(_baseUpgradeCost * Mathf.Pow(_costMultiplier, level));
        }

        public float GetDPS(int level)
        {
            return _baseDPS * Mathf.Pow(_dpsMultiplier, level);
        }
        
    }
}