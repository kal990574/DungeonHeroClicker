using UnityEngine;
using System;
using _01.Scripts.Core.Utils;

namespace _01.Scripts.Ingame.Monster
{
    public class MonsterReward : MonoBehaviour
    {
        [Header("Reward")]
        [SerializeField] private long _goldAmount = 10;

        private MonsterHealth _health;
        private BigNumber _calculatedGoldAmount;

        public BigNumber GoldAmount => _calculatedGoldAmount;

        public static event Action<BigNumber, Vector3> OnGoldDropped;

        private void Awake()
        {
            _health = GetComponent<MonsterHealth>();
            _calculatedGoldAmount = new BigNumber(_goldAmount);
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
            OnGoldDropped?.Invoke(_calculatedGoldAmount, transform.position);
        }

        public void SetGoldAmount(BigNumber amount)
        {
            _calculatedGoldAmount = amount;
        }
    }
}