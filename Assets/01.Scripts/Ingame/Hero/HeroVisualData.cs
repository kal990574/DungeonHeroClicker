using System;
using UnityEngine;

namespace _01.Scripts.Ingame.Hero
{
    [CreateAssetMenu(fileName = "HeroVisualData", menuName = "DungeonHeroClicker/Hero Visual Data")]
    public class HeroVisualData : ScriptableObject
    {
        [SerializeField] private VisualTier[] _tiers;

        public VisualTier GetTier(int level)
        {
            for (int i = _tiers.Length - 1; i >= 0; i--)
            {
                if (level >= _tiers[i].MinLevel)
                {
                    return _tiers[i];
                }
            }

            return _tiers[0];
        }
    }
    
    [Serializable]
    public class VisualTier
    {
        [Header("Level Requirement")]
        public int MinLevel;

        [Header("Visuals")]
        public string TierName;
        public GameObject HeroPrefab;
    }
}