using System;
using Firebase.Firestore;

namespace _01.Scripts.Outgame.Stage.Repo
{
    [Serializable]
    [FirestoreData]
    public class StageSaveData
    {
        [FirestoreProperty]
        public int CurrentStage { get; set; }

        [FirestoreProperty]
        public int CurrentKillCount { get; set; }
    }
}