using UnityEngine;
using System;

namespace _01.Scripts.Ingame.Monster
{
    public class MonsterReward : MonoBehaviour
    {
        [Header("Reward")]
        [SerializeField] private int _goldAmount = 10;

        private MonsterHealth _health;

        public int GoldAmount => _goldAmount;

        public static event Action<int> OnGoldDropped;

        private void Awake()
        {
            _health = GetComponent<MonsterHealth>();
        }

        private void OnEnable()
        {
            if (_health != null)
            {
                _health.OnDeath += DropReward;
            }
        }

        private void OnDisable()
        {
            if (_health != null)
            {
                _health.OnDeath -= DropReward;
            }
        }

        private void DropReward()
        {
            OnGoldDropped?.Invoke(_goldAmount);
        }
        
        private void SetGoldAmount(int amount)
        {
            _goldAmount = amount;
        }

    }
}