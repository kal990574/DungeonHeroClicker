#if !UNITY_WEBGL
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using _01.Scripts.Interfaces.Upgrade;
using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade.Repo
{
    public class FirebaseUpgradeRepository : IUpgradeRepository
    {
        private const string FieldName = "Upgrade";

        private readonly FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
        private readonly FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;

        public async UniTask Save(UpgradeSaveData data)
        {
            try
            {
                string email = _auth.CurrentUser.Email;
                var dict = new Dictionary<string, object> { { FieldName, data } };
                await _db.Collection("users").Document(email).SetAsync(dict, SetOptions.MergeAll);
            }
            catch (Exception e)
            {
                Debug.LogError("[FirebaseUpgradeRepository] Save 실패: " + e.Message);
            }
        }

        public async UniTask<UpgradeSaveData> Load()
        {
            try
            {
                string email = _auth.CurrentUser.Email;
                DocumentSnapshot snapshot = await _db.Collection("users").Document(email).GetSnapshotAsync();

                if (!snapshot.Exists || !snapshot.ContainsField(FieldName))
                {
                    return null;
                }

                return snapshot.GetValue<UpgradeSaveData>(FieldName);
            }
            catch (Exception e)
            {
                Debug.LogError("[FirebaseUpgradeRepository] Load 실패: " + e.Message);
                return null;
            }
        }
    }
}
#endif