using _01.Scripts.Core.Audio;
using _01.Scripts.Core.Utils;
using _01.Scripts.Ingame.Monster;
using _01.Scripts.UI.Effects;
using UnityEngine;

namespace _01.Scripts.Outgame.Currency
{
    public class CurrencyManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private GoldFlyEffect _goldFlyEffect;

        private void OnEnable()
        {
            MonsterReward.OnGoldDropped += HandleGoldDropped;
        }

        private void OnDisable()
        {
            MonsterReward.OnGoldDropped -= HandleGoldDropped;
        }

        private void HandleGoldDropped(BigNumber amount, Vector3 worldPosition)
        {
            if (_goldFlyEffect != null)
            {
                _goldFlyEffect.Play(worldPosition, amount, OnCoinArrived);
            }
            else
            {
                _goldWallet.Add(amount);
                SFXManager.Instance?.PlayCoin();
            }
        }

        private void OnCoinArrived(BigNumber goldValue)
        {
            _goldWallet.Add(goldValue);
            SFXManager.Instance?.PlayCoin();
        }
    }
}