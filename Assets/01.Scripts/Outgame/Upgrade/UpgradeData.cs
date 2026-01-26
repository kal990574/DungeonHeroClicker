using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade
{
    [CreateAssetMenu(fileName = "UpgradeData", menuName = "DungeonHeroClicker/Upgrade Data")]
    public class UpgradeData : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private string _upgradeName;
        [SerializeField] private Sprite _icon;

        [Header("Cost")]
        [SerializeField] private int _baseCost = 100;
        [SerializeField] private float _costMultiplier = 1.15f;

        [Header("Effect")]
        [SerializeField] private float _baseEffect = 5f;
        [SerializeField] private float _effectPerLevel = 5f;

        public string UpgradeName => _upgradeName;
        public Sprite Icon => _icon;
        public int BaseCost => _baseCost;
        public float CostMultiplier => _costMultiplier;
        public float BaseEffect => _baseEffect;
        public float EffectPerLevel => _effectPerLevel;

        public int GetCost(int level)
        {
            return Mathf.RoundToInt(_baseCost * Mathf.Pow(_costMultiplier, level));
        }

        public float GetEffect(int level)
        {
            return _baseEffect + (_effectPerLevel * level);
        }
    }
}