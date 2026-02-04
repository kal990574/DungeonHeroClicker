using System;
using Firebase.Firestore;
using _01.Scripts.Outgame.Currency.Domain;

namespace _01.Scripts.Outgame.Currency.Repo
{
    [Serializable]
    [FirestoreData]
    public class CurrencySaveData
    {
        [FirestoreProperty]
        public double[] Mantissas { get; set; }

        [FirestoreProperty]
        public long[] Exponents { get; set; }

        public static CurrencySaveData Default => new CurrencySaveData
        {
            Mantissas = new double[(int)ECurrencyType.Count],
            Exponents = new long[(int)ECurrencyType.Count]
        };
    }
}