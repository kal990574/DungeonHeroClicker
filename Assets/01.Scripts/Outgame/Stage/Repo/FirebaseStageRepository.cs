using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using _01.Scripts.Interfaces;

namespace _01.Scripts.Outgame.Stage.Repo
{
    public class FirebaseStageRepository : IFirebaseStageRepository
    {
        private readonly DocumentReference _docRef;

        public FirebaseStageRepository(FirebaseFirestore db, string userId)
        {
            _docRef = db.Collection("users").Document(userId).Collection("saves").Document("stage");
        }

        public async UniTask Save(StageSaveData data)
        {
            var dict = new Dictionary<string, object>
            {
                { "CurrentStage", data.CurrentStage },
                { "CurrentKillCount", data.CurrentKillCount }
            };

            await _docRef.SetAsync(dict).AsUniTask();
        }

        public async UniTask<StageSaveData> Load()
        {
            DocumentSnapshot snapshot = await _docRef.GetSnapshotAsync().AsUniTask();

            if (!snapshot.Exists)
            {
                return null;
            }

            return new StageSaveData
            {
                CurrentStage = Convert.ToInt32(snapshot.GetValue<object>("CurrentStage")),
                CurrentKillCount = Convert.ToInt32(snapshot.GetValue<object>("CurrentKillCount"))
            };
        }
    }
}