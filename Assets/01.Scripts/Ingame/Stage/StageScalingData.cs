using UnityEngine;

namespace _01.Scripts.Ingame.Stage
{
    [CreateAssetMenu(fileName = "StageScalingData", menuName = "DungeonHeroClicker/Stage Scaling Data")]
    public class StageScalingData : ScriptableObject
    {
        [Header("Base Values")]
        [SerializeField] private float _baseHealth = 100f;
        [SerializeField] private int _baseGold = 10;

        [Header("Growth Rates")]
        [Tooltip("10% = 0.1")]
        [SerializeField] private float _healthGrowthRate = 0.1f;
        [SerializeField] private float _goldGrowthRate = 0.1f;

        public float BaseHealth => _baseHealth;
        public int BaseGold => _baseGold;
        public float HealthGrowthRate => _healthGrowthRate;
        public float GoldGrowthRate => _goldGrowthRate;
    }
}