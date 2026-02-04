using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using _01.Scripts.Interfaces.Currency;
using UnityEngine;

namespace _01.Scripts.Outgame.Currency.Repo
{
    public class FirebaseCurrencyRepository : ICurrencyRepository
    {
        private const string FieldName = "Currency";

        private readonly FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
        private readonly FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;

        public async UniTask Save(CurrencySaveData data)
        {
            try
            {
                string email = _auth.CurrentUser.Email;
                var dict = new Dictionary<string, object> { { FieldName, data } };
                await _db.Collection("users").Document(email).SetAsync(dict, SetOptions.MergeAll);
            }
            catch (Exception e)
            {
                Debug.LogError("[FirebaseCurrencyRepository] Save 실패: " + e.Message);
            }
        }

        public async UniTask<CurrencySaveData> Load()
        {
            try
            {
                string email = _auth.CurrentUser.Email;
                DocumentSnapshot snapshot = await _db.Collection("users").Document(email).GetSnapshotAsync();

                if (!snapshot.Exists || !snapshot.ContainsField(FieldName))
                {
                    return null;
                }

                return snapshot.GetValue<CurrencySaveData>(FieldName);
            }
            catch (Exception e)
            {
                Debug.LogError("[FirebaseCurrencyRepository] Load 실패: " + e.Message);
                return null;
            }
        }
    }
}