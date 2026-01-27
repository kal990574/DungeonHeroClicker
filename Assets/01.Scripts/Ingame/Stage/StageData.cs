using UnityEngine;

namespace _01.Scripts.Ingame.Stage
{
    [CreateAssetMenu(fileName = "StageData", menuName = "DungeonHeroClicker/Stage Data")]
    public class StageData : ScriptableObject
    {
        [Header("Stage Settings")]
        [SerializeField] private int _monstersPerStage = 10;
        [SerializeField] private int _bossInterval = 5;

        [Header("Boss Multipliers")]
        [SerializeField] private float _bossHealthMultiplier = 5f;
        [SerializeField] private float _bossGoldMultiplier = 10f;

        public int MonstersPerStage => _monstersPerStage;
        public int BossInterval => _bossInterval;
        public float BossHealthMultiplier => _bossHealthMultiplier;
        public float BossGoldMultiplier => _bossGoldMultiplier;

        public bool IsBossStage(int stage)
        {
            return stage > 0 && stage % _bossInterval == 0;
        }
    }
}