using UnityEngine;

namespace _01.Scripts.Outgame.Stage.Config
{
    [CreateAssetMenu(fileName = "StageScalingData", menuName = "DungeonHeroClicker/Stage Scaling Data")]
    public class StageScalingData : ScriptableObject
    {
        [Header("Base Values")]
        [SerializeField] private float _baseHealth = 100f;
        [SerializeField] private long _baseGold = 10;

        [Header("Growth Multipliers")]
        [Tooltip("15% 증가 = 1.15, 공식: base × rate^(stage-1)")]
        [SerializeField] private float _healthGrowthRate = 1.15f;
        [SerializeField] private float _goldGrowthRate = 1.15f;

        public float BaseHealth => _baseHealth;
        public long BaseGold => _baseGold;
        public float HealthGrowthRate => _healthGrowthRate;
        public float GoldGrowthRate => _goldGrowthRate;
    }
}