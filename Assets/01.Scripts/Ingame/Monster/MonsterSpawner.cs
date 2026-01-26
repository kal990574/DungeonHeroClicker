using System;
using UnityEngine;

namespace _01.Scripts.Ingame.Monster
{
    public class MonsterSpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Monster _monsterPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _respawnDelay = 0.5f;

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
            OnMonsterSpawned?.Invoke(_currentMonster);
        }

        private void HandleMonsterDeath()
        {
            _currentMonster.OnMonsterDeath -= HandleMonsterDeath;
            Invoke(nameof(SpawnMonster), _respawnDelay);
        }
    }
}