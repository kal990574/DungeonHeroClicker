using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using _01.Scripts.Interfaces;
using UnityEngine;

namespace _01.Scripts.Outgame.Stage.Repo
{
    public class FirebaseStageRepository : IStageRepository
    {
        private const string FieldName = "Stage";

        private readonly FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
        private readonly FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;

        public async UniTask Save(StageSaveData data)
        {
            try
            {
                string email = _auth.CurrentUser.Email;
                var dict = new Dictionary<string, object> { { FieldName, data } };
                await _db.Collection("users").Document(email).SetAsync(dict, SetOptions.MergeAll);
            }
            catch (Exception e)
            {
                Debug.LogError("[FirebaseStageRepository] Save 실패: " + e.Message);
            }
        }

        public async UniTask<StageSaveData> Load()
        {
            try
            {
                string email = _auth.CurrentUser.Email;
                DocumentSnapshot snapshot = await _db.Collection("users").Document(email).GetSnapshotAsync();

                if (!snapshot.Exists || !snapshot.ContainsField(FieldName))
                {
                    return null;
                }

                return snapshot.GetValue<StageSaveData>(FieldName);
            }
            catch (Exception e)
            {
                Debug.LogError("[FirebaseStageRepository] Load 실패: " + e.Message);
                return null;
            }
        }
    }
}