using System;
using System.Collections.Generic;
#if !UNITY_WEBGL
using Firebase.Firestore;
#endif

namespace _01.Scripts.Outgame.Upgrade.Repo
{
    [Serializable]
#if !UNITY_WEBGL
    [FirestoreData]
#endif
    public class UpgradeSaveData
    {
#if !UNITY_WEBGL
        [FirestoreProperty]
#endif
        public List<UpgradeStateEntry> Entries { get; set; } = new List<UpgradeStateEntry>();
    }

    [Serializable]
#if !UNITY_WEBGL
    [FirestoreData]
#endif
    public class UpgradeStateEntry
    {
#if !UNITY_WEBGL
        [FirestoreProperty]
#endif
        public string Id { get; set; }

#if !UNITY_WEBGL
        [FirestoreProperty]
#endif
        public int Type { get; set; }

#if !UNITY_WEBGL
        [FirestoreProperty]
#endif
        public int CurrentLevel { get; set; }

#if !UNITY_WEBGL
        [FirestoreProperty]
#endif
        public bool IsPurchased { get; set; }
    }
}