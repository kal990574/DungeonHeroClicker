using UnityEngine;
using _01.Scripts.Interfaces;

namespace _01.Scripts.Ingame.Stage
{
    public class StageStatCalculator : IMonsterStatModifier
    {
        private readonly StageScalingData _scalingData;
        private readonly StageData _stageData;

        public StageStatCalculator(StageScalingData scalingData, StageData stageData)
        {
            _scalingData = scalingData;
            _stageData = stageData;
        }

        public float CalculateHealth(float baseHealth, int stage, bool isBoss)
        {
            float scaledHealth = baseHealth * Mathf.Pow(1f + _scalingData.HealthGrowthRate, stage - 1);

            if (isBoss)
            {
                scaledHealth *= _stageData.BossHealthMultiplier;
            }

            return Mathf.Round(scaledHealth);
        }

        public int CalculateGold(int baseGold, int stage, bool isBoss)
        {
            float scaledGold = baseGold * Mathf.Pow(1f + _scalingData.GoldGrowthRate, stage - 1);

            if (isBoss)
            {
                scaledGold *= _stageData.BossGoldMultiplier;
            }

            return Mathf.RoundToInt(scaledGold);
        }
    }
}