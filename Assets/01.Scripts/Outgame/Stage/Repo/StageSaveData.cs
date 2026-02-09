using System;
#if !UNITY_WEBGL
using Firebase.Firestore;
#endif

namespace _01.Scripts.Outgame.Stage.Repo
{
    [Serializable]
#if !UNITY_WEBGL
    [FirestoreData]
#endif
    public class StageSaveData
    {
#if !UNITY_WEBGL
        [FirestoreProperty]
#endif
        public int CurrentStage { get; set; }

#if !UNITY_WEBGL
        [FirestoreProperty]
#endif
        public int CurrentKillCount { get; set; }
    }
}