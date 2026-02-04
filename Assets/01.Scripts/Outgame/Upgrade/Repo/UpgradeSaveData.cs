using System;
using System.Collections.Generic;
using Firebase.Firestore;

namespace _01.Scripts.Outgame.Upgrade.Repo
{
    [Serializable]
    [FirestoreData]
    public class UpgradeSaveData
    {
        [FirestoreProperty]
        public List<UpgradeStateEntry> Entries { get; set; } = new List<UpgradeStateEntry>();
    }

    [Serializable]
    [FirestoreData]
    public class UpgradeStateEntry
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public int Type { get; set; }

        [FirestoreProperty]
        public int CurrentLevel { get; set; }

        [FirestoreProperty]
        public bool IsPurchased { get; set; }
    }
}