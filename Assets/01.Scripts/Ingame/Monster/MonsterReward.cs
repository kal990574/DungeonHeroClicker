using UnityEngine;
using System;
using _01.Scripts.Core.Utils;

namespace _01.Scripts.Ingame.Monster
{
    public class MonsterReward : MonoBehaviour
    {
        [Header("Reward")]
        [SerializeField] private long _goldAmount = 10;

        private Monster _monster;
        private BigNumber _calculatedGoldAmount;

        public static event Action<BigNumber, Vector3> OnGoldDropped;

        private void Awake()
        {
            _monster = GetComponent<Monster>();
            _calculatedGoldAmount = new BigNumber(_goldAmount);
        }

        private void OnEnable()
        {
            if (_monster != null)
            {
                _monster.OnMonsterDeath += DropReward;
            }
        }

        private void OnDisable()
        {
            if (_monster != null)
            {
                _monster.OnMonsterDeath -= DropReward;
            }
        }

        private void DropReward()
        {
            OnGoldDropped?.Invoke(_calculatedGoldAmount, transform.position);
        }

        public void SetGoldAmount(BigNumber amount)
        {
            _calculatedGoldAmount = amount;
        }
    }
}