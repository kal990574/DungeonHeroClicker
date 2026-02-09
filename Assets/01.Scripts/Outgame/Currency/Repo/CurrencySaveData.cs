using System;
#if !UNITY_WEBGL
using Firebase.Firestore;
#endif
using _01.Scripts.Outgame.Currency.Domain;

namespace _01.Scripts.Outgame.Currency.Repo
{
    [Serializable]
#if !UNITY_WEBGL
    [FirestoreData]
#endif
    public class CurrencySaveData
    {
#if !UNITY_WEBGL
        [FirestoreProperty]
#endif
        public double[] Mantissas { get; set; }

#if !UNITY_WEBGL
        [FirestoreProperty]
#endif
        public long[] Exponents { get; set; }

        public static CurrencySaveData Default => new CurrencySaveData
        {
            Mantissas = new double[(int)ECurrencyType.Count],
            Exponents = new long[(int)ECurrencyType.Count]
        };
    }
}