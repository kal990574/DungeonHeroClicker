using System;
using _01.Scripts.Ingame.Click;
using _01.Scripts.Ingame.Stage;
using _01.Scripts.UI;
using UnityEngine;

namespace _01.Scripts.Ingame.Monster
{
    public class MonsterSpawner : MonoBehaviour
    {
        [Header("Monster Prefabs")]
        [SerializeField] private Monster[] _monsterPrefabs;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _respawnDelay = 0.5f;

        [Header("UI")]
        [SerializeField] private MonsterHealthBar _healthBar;

        [Header("Auto Click")]
        [SerializeField] private AutoClickManager _autoClickManager;

        [Header("Stage System")]
        [SerializeField] private StageManager _stageManager;

        private Monster _currentMonster;
        private int _currentIndex;

        private void Start()
        {
            _currentIndex = 0;
            SpawnMonster();
        }

        public void SpawnMonster()
        {
            if (_monsterPrefabs == null || _monsterPrefabs.Length == 0)
            {
                Debug.LogError("[MonsterSpawner] No monster prefabs assigned!");
                return;
            }

            if (_currentMonster != null && !_currentMonster.IsDead)
            {
                return;
            }

            // 이전 몬스터 제거
            if (_currentMonster != null)
            {
                Destroy(_currentMonster.gameObject);
            }

            // 현재 인덱스의 프리팹으로 새 몬스터 생성
            var prefab = _monsterPrefabs[_currentIndex];
            _currentMonster = Instantiate(prefab, _spawnPoint.position, Quaternion.identity);
            _currentMonster.OnMonsterDeath += HandleMonsterDeath;

            // 스테이지 기반 스탯 적용
            ConfigureMonsterStats(_currentMonster);

            // 체력바 타겟 설정
            var health = _currentMonster.GetComponent<MonsterHealth>();
            if (_healthBar != null && health != null)
            {
                _healthBar.SetTarget(health);
            }

            // 자동 공격 타겟 설정
            if (_autoClickManager != null)
            {
                _autoClickManager.SetTarget(_currentMonster, _currentMonster.transform);
            }
        }

        private void HandleMonsterDeath()
        {
            _currentMonster.OnMonsterDeath -= HandleMonsterDeath;

            if (_autoClickManager != null)
            {
                _autoClickManager.ClearTarget();
            }

            // 스테이지 매니저에 처치 알림
            if (_stageManager != null)
            {
                _stageManager.OnMonsterKilled();
            }

            // 다음 몬스터로 인덱스 증가
            _currentIndex = (_currentIndex + 1) % _monsterPrefabs.Length;

            Invoke(nameof(SpawnMonster), _respawnDelay);
        }

        private void ConfigureMonsterStats(Monster monster)
        {
            if (_stageManager == null)
            {
                return;
            }

            var health = monster.GetComponent<MonsterHealth>();
            var reward = monster.GetComponent<MonsterReward>();
            var calculator = _stageManager.StatCalculator;

            bool isBoss = _stageManager.IsNextMonsterBoss;
            int stage = _stageManager.CurrentStage;

            if (health != null && calculator != null)
            {
                float baseHealth = calculator.BaseHealth * monster.HealthMultiplier;
                var scaledHealth = calculator.CalculateHealth(baseHealth, stage, isBoss);
                health.SetMaxHealth(scaledHealth);
            }

            if (reward != null && calculator != null)
            {
                long baseGold = (long)(calculator.BaseGold * monster.GoldMultiplier);
                var scaledGold = calculator.CalculateGold(baseGold, stage, isBoss);
                reward.SetGoldAmount(scaledGold);
            }
        }
    }
}