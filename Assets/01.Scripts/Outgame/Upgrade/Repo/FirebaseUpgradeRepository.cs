using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using _01.Scripts.Interfaces.Upgrade;

namespace _01.Scripts.Outgame.Upgrade.Repo
{
    public class FirebaseUpgradeRepository : IFirebaseUpgradeRepository
    {
        private readonly DocumentReference _docRef;

        public FirebaseUpgradeRepository(FirebaseFirestore db, string userId)
        {
            _docRef = db.Collection("users").Document(userId).Collection("saves").Document("upgrade");
        }

        public async UniTask Save(UpgradeSaveData data)
        {
            var entries = data.Entries.Select(entry => new Dictionary<string, object>
            {
                { "Id", entry.Id },
                { "Type", entry.Type },
                { "CurrentLevel", entry.CurrentLevel },
                { "IsPurchased", entry.IsPurchased }
            }).ToList();

            var dict = new Dictionary<string, object>
            {
                { "Entries", entries }
            };

            await _docRef.SetAsync(dict).AsUniTask();
        }

        public async UniTask<UpgradeSaveData> Load()
        {
            DocumentSnapshot snapshot = await _docRef.GetSnapshotAsync().AsUniTask();

            if (!snapshot.Exists)
            {
                return null;
            }

            var rawEntries = snapshot.GetValue<List<object>>("Entries");

            var data = new UpgradeSaveData();
            foreach (Dictionary<string, object> raw in rawEntries)
            {
                data.Entries.Add(new UpgradeStateEntry
                {
                    Id = Convert.ToString(raw["Id"]),
                    Type = Convert.ToInt32(raw["Type"]),
                    CurrentLevel = Convert.ToInt32(raw["CurrentLevel"]),
                    IsPurchased = Convert.ToBoolean(raw["IsPurchased"])
                });
            }

            return data;
        }
    }
}