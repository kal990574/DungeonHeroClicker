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
        [SerializeField] private float _dpsPerLevel = 1f;

        [Header("Upgrade Cost")]
        [SerializeField] private int _baseUpgradeCost = 100;
        [SerializeField] private float _costMultiplier = 1.15f;

        public string CompanionName => _companionName;
        public Sprite Icon => _icon;
        public int PurchaseCost => _purchaseCost;
        public float BaseDPS => _baseDPS;
        public float DPSPerLevel => _dpsPerLevel;

        public int GetUpgradeCost(int level)
        {
            return Mathf.RoundToInt(_baseUpgradeCost * Mathf.Pow(_costMultiplier, level));
        }

        public float GetDPS(int level)
        {
            return _baseDPS + (_dpsPerLevel * level);
        }
        
    }
}