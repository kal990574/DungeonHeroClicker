using UnityEngine;

namespace _01.Scripts.Ingame.Hero
{
    public class HeroVisual : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private HeroVisualData _visualData;

        [Header("Spawn Point")]
        [SerializeField] private Transform _spawnPoint;

        private int _currentTierIndex = -1;
        private GameObject _currentHero;

        public GameObject CurrentHero => _currentHero;

        private void Start()
        {
            if (_spawnPoint == null)
            {
                _spawnPoint = transform;
            }

            UpdateVisual(1);
        }

        public void UpdateVisual(int level)
        {
            var tier = _visualData.GetTier(level);
            int newTierIndex = tier.MinLevel;

            if (newTierIndex == _currentTierIndex)
            {
                return;
            }

            _currentTierIndex = newTierIndex;

            // 기존 프리팹 제거.
            if (_currentHero != null)
            {
                Destroy(_currentHero);
            }

            // 새 프리팹 생성.
            if (tier.HeroPrefab != null)
            {
                _currentHero = Instantiate(tier.HeroPrefab, _spawnPoint.position, Quaternion.identity, _spawnPoint);
            }

            Debug.Log($"[HeroVisual] Tier Changed: {tier.TierName}");
        }
    }
}