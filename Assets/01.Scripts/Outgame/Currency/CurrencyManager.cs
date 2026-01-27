using _01.Scripts.Core.Audio;
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

        private void HandleGoldDropped(int amount, Vector3 worldPosition)
        {
            _goldWallet.Add(amount);
            SFXManager.Instance?.PlayCoin();
            _goldFlyEffect?.Play(worldPosition, amount);
        }
    }
}