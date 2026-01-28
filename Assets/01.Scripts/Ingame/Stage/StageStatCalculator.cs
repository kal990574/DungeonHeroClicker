using UnityEngine;
using _01.Scripts.Core.Utils;
using _01.Scripts.Interfaces;

namespace _01.Scripts.Ingame.Stage
{
    public class StageStatCalculator : IMonsterStatModifier
    {
        private readonly StageScalingData _scalingData;
        private readonly StageData _stageData;

        public float BaseHealth => _scalingData.BaseHealth;
        public long BaseGold => _scalingData.BaseGold;

        public StageStatCalculator(StageScalingData scalingData, StageData stageData)
        {
            _scalingData = scalingData;
            _stageData = stageData;
        }

        public BigNumber CalculateHealth(float baseHealth, int stage, bool isBoss)
        {
            double scaledHealth = baseHealth * System.Math.Pow(_scalingData.HealthGrowthRate, stage - 1);

            if (isBoss)
            {
                scaledHealth *= _stageData.BossHealthMultiplier;
            }

            return new BigNumber(System.Math.Round(scaledHealth));
        }

        public BigNumber CalculateGold(long baseGold, int stage, bool isBoss)
        {
            double scaledGold = baseGold * System.Math.Pow(_scalingData.GoldGrowthRate, stage - 1);

            if (isBoss)
            {
                scaledGold *= _stageData.BossGoldMultiplier;
            }

            return new BigNumber(System.Math.Round(scaledGold));
        }
    }
}