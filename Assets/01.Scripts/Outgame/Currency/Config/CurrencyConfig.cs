using _01.Scripts.Outgame.Currency.Domain;
using UnityEngine;

namespace _01.Scripts.Outgame.Currency.Config
{
    [CreateAssetMenu(fileName = "CurrencyConfig", menuName = "DungeonHeroClicker/Config/Currency")]
    public class CurrencyConfig : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private ECurrencyType _type;

        [Header("Settings")]
        [SerializeField] private long _initialAmount;

        public ECurrencyType Type => _type;
        public long InitialAmount => _initialAmount;
    }
}