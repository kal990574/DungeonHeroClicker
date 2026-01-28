using UnityEngine;
using _01.Scripts.Core.Utils;

namespace _01.Scripts.Outgame.Upgrade
{
    [CreateAssetMenu(fileName = "UpgradeData", menuName = "DungeonHeroClicker/Upgrade Data")]
    public class UpgradeData : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private string _upgradeName;
        [SerializeField] private Sprite _icon;

        [Header("Cost")]
        [SerializeField] private long _baseCost = 100;
        [SerializeField] private float _costMultiplier = 1.15f;

        [Header("Effect")]
        [SerializeField] private float _baseEffect = 5f;
        [SerializeField] private float _effectMultiplier = 1.15f;

        public string UpgradeName => _upgradeName;
        public Sprite Icon => _icon;
        public long BaseCost => _baseCost;
        public float CostMultiplier => _costMultiplier;
        public float BaseEffect => _baseEffect;
        public float EffectMultiplier => _effectMultiplier;

        public BigNumber GetCost(int level)
        {
            double cost = _baseCost * System.Math.Pow(_costMultiplier, level);
            return new BigNumber(System.Math.Round(cost));
        }

        public BigNumber GetEffect(int level)
        {
            double effect = _baseEffect * System.Math.Pow(_effectMultiplier, level);
            return new BigNumber(effect);
        }
    }
}