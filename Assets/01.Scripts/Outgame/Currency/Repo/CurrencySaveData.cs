using System;
using _01.Scripts.Outgame.Currency.Domain;

namespace _01.Scripts.Outgame.Currency.Repo
{
    [Serializable]
    public class CurrencySaveData
    {
        public double[] Mantissas;
        public long[] Exponents;

        public static CurrencySaveData Default => new CurrencySaveData
        {
            Mantissas = new double[(int)ECurrencyType.Count],
            Exponents = new long[(int)ECurrencyType.Count]
        };
    }
}