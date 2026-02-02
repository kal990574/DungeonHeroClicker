using System;
using UnityEngine;

namespace _01.Scripts.Ingame.Hero
{
    public class HeroVisual : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private HeroVisualData _visualData;

        [Header("Spawn Point")]
        [SerializeField] private Transform _spawnPoint;

        [Header("Animation")]
        [SerializeField] private HeroAnimator _heroAnimator;

        private int _currentTierIndex = -1;
        private string _currentTierName;
        private GameObject _currentHero;

        public event Action<TierChangeInfo> OnTierChanged;

        public GameObject CurrentHero => _currentHero;

        private void Awake()
        {
            if (_spawnPoint == null)
            {
                _spawnPoint = transform;
            }

            if (_heroAnimator == null)
            {
                _heroAnimator = GetComponent<HeroAnimator>();
            }
        }

        public void UpdateVisual(int level)
        {
            var tier = _visualData.GetTier(level);
            int newTierIndex = tier.MinLevel;

            if (newTierIndex == _currentTierIndex)
            {
                return;
            }

            var previousTierName = _currentTierName;
            _currentTierIndex = newTierIndex;
            _currentTierName = tier.TierName;

            // 기존 프리팹 제거.
            if (_currentHero != null)
            {
                Destroy(_currentHero);
            }

            // 새 프리팹 생성.
            if (tier.HeroPrefab != null)
            {
                _currentHero = Instantiate(tier.HeroPrefab, _spawnPoint.position, Quaternion.identity, _spawnPoint);

                // 오른쪽(몬스터 방향)을 바라보도록 설정.
                _currentHero.transform.localScale = new Vector3(-1, 1, 1);

                // HeroAnimator에 SPUM_Prefabs 전달.
                var spumPrefabs = _currentHero.GetComponent<SPUM_Prefabs>();
                if (_heroAnimator != null && spumPrefabs != null)
                {
                    _heroAnimator.Initialize(spumPrefabs);
                }
            }

            // 티어 변경 이벤트 발행 (첫 초기화가 아닌 경우에만).
            if (!string.IsNullOrEmpty(previousTierName))
            {
                OnTierChanged?.Invoke(new TierChangeInfo(previousTierName, tier.TierName, tier.MinLevel));
            }

            Debug.Log($"[HeroVisual] Tier Changed: {tier.TierName}");
        }
    }
}