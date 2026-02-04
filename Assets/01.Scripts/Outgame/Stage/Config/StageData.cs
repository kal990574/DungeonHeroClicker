using UnityEngine;

namespace _01.Scripts.Outgame.Stage.Config
{
    [CreateAssetMenu(fileName = "StageData", menuName = "DungeonHeroClicker/Stage Data")]
    public class StageData : ScriptableObject
    {
        [Header("Stage Settings")]
        [SerializeField] private int _monstersPerStage = 10;

        [Header("Boss Multipliers")]
        [SerializeField] private float _bossHealthMultiplier = 5f;
        [SerializeField] private float _bossGoldMultiplier = 10f;

        public int MonstersPerStage => _monstersPerStage;
        public float BossHealthMultiplier => _bossHealthMultiplier;
        public float BossGoldMultiplier => _bossGoldMultiplier;
    }
}