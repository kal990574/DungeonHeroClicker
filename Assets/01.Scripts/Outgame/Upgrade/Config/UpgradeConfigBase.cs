using _01.Scripts.Outgame.Upgrade.Domain;
using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade.Config
{
    public abstract class UpgradeConfigBase : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private Sprite _icon;
        [SerializeField] private EUpgradeType _type;

        [Header("Upgrade Cost")]
        [SerializeField] private long _baseCost = 100;
        [SerializeField] private float _costMultiplier = 1.15f;

        [Header("Effect")]
        [SerializeField] private float _baseEffect = 5f;
        [SerializeField] private float _effectMultiplier = 1.15f;

        public string Id => _id;
        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
        public EUpgradeType Type => _type;
        public long BaseCost => _baseCost;
        public float CostMultiplier => _costMultiplier;
        public float BaseEffect => _baseEffect;
        public float EffectMultiplier => _effectMultiplier;
    }
}