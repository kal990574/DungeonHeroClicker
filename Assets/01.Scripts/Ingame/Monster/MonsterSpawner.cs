using System;
using _01.Scripts.Ingame.Click;
using _01.Scripts.UI;
using UnityEngine;

namespace _01.Scripts.Ingame.Monster
{
    public class MonsterSpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Monster _monsterPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _respawnDelay = 0.5f;

        [Header("UI")]
        [SerializeField] private MonsterHealthBar _healthBar;

        [Header("Auto Click")]
        [SerializeField] private AutoClickManager _autoClickManager;

        private Monster _currentMonster;

        public Monster CurrentMonster => _currentMonster;

        public event Action<Monster> OnMonsterSpawned;

        private void Start()
        {
            SpawnMonster();
        }

        public void SpawnMonster()
        {
            if (_currentMonster != null && !_currentMonster.IsDead)
            {
                return;
            }

            if (_currentMonster == null)
            {
                _currentMonster = Instantiate(_monsterPrefab, _spawnPoint.position, Quaternion.identity);
            }
            else
            {
                _currentMonster.Reset();
                _currentMonster.transform.position = _spawnPoint.position;
            }

            _currentMonster.OnMonsterDeath += HandleMonsterDeath;

            // 체력바 타겟 설정.
            var health = _currentMonster.GetComponent<MonsterHealth>();
            if (_healthBar != null && health != null)
            {
                _healthBar.SetTarget(health);
            }

            // 자동 공격 타겟 설정.
            if (_autoClickManager != null)
            {
                _autoClickManager.SetTarget(_currentMonster, _currentMonster.transform);
            }

            OnMonsterSpawned?.Invoke(_currentMonster);
        }

        private void HandleMonsterDeath()
        {
            _currentMonster.OnMonsterDeath -= HandleMonsterDeath;

            if (_autoClickManager != null)
            {
                _autoClickManager.ClearTarget();
            }

            Invoke(nameof(SpawnMonster), _respawnDelay);
        }
    }
}