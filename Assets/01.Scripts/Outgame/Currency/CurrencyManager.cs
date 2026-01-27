using _01.Scripts.Ingame.Monster;
using _01.Scripts.Core.Audio;
using UnityEngine;

namespace _01.Scripts.Outgame.Currency
{
    public class CurrencyManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GoldWallet _goldWallet;

        private void OnEnable()
        {
            MonsterReward.OnGoldDropped += HandleGoldDropped;
        }

        private void OnDisable()
        {
            MonsterReward.OnGoldDropped -= HandleGoldDropped;
        }

        private void HandleGoldDropped(int amount)
        {
            _goldWallet.Add(amount);
            SFXManager.Instance?.PlayCoin();
            Debug.Log($"[CurrencyManager] Gold +{amount}, Total: {_goldWallet.CurrentGold}");
        }
    }
}