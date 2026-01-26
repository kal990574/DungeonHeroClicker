using UnityEngine;

namespace _01.Scripts.Ingame.Hero
{
    public class HeroVisual :  MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private HeroVisualData _visualData;

        [Header("Renderers")]
        [SerializeField] private SpriteRenderer _heroRenderer;
        [SerializeField] private SpriteRenderer _weaponRenderer;

        private int _currentTierIndex = -1;
        
        public void UpdateVisual(int level)
        {
            var tier = _visualData.GetTier(level);
            int newTierIndex = tier.MinLevel;

            if (newTierIndex == _currentTierIndex)
            {
                return;
            }

            _currentTierIndex = newTierIndex;
            _heroRenderer.sprite = tier.HeroSprite;
            _weaponRenderer.sprite = tier.WeaponSprite;

            Debug.Log($"[HeroVisual] Tier Changed: {tier.TierName}");
        }
    }
}