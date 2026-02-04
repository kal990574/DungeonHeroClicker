using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using _01.Scripts.Interfaces.Currency;
using UnityEngine;

namespace _01.Scripts.Outgame.Currency.Repo
{
    public class FirebaseCurrencyRepository : IFirebaseCurrencyRepository
    {
        private readonly DocumentReference _docRef;

        public FirebaseCurrencyRepository(FirebaseFirestore db, string userId)
        {
            _docRef = db.Collection("users").Document(userId).Collection("saves").Document("currency");
        }

        public async UniTask Save(CurrencySaveData data)
        {
            var dict = new Dictionary<string, object>
            {
                { "Mantissas", data.Mantissas.ToList() },
                { "Exponents", data.Exponents.ToList() }
            };

            await _docRef.SetAsync(dict).AsUniTask();
        }

        public async UniTask<CurrencySaveData> Load()
        {
            DocumentSnapshot snapshot = await _docRef.GetSnapshotAsync().AsUniTask();

            if (!snapshot.Exists)
            {
                return null;
            }

            var rawMantissas = snapshot.GetValue<List<object>>("Mantissas");
            var rawExponents = snapshot.GetValue<List<object>>("Exponents");

            return new CurrencySaveData
            {
                Mantissas = rawMantissas.Select(v => Convert.ToDouble(v)).ToArray(),
                Exponents = rawExponents.Select(v => Convert.ToInt64(v)).ToArray()
            };
        }
    }
}