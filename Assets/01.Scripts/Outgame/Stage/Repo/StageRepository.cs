using System.IO;
using Cysharp.Threading.Tasks;
using _01.Scripts.Core.Utils;
using _01.Scripts.Interfaces;
using _01.Scripts.Outgame.Account.Manager;
using UnityEngine;

namespace _01.Scripts.Outgame.Stage.Repo
{
    public class StageRepository : IStageRepository
    {
        private readonly string _savePath;

        public StageRepository()
        {
            string accountId = AccountManager.Instance.CurrentAccountId;
            string directory = Path.Combine(Application.persistentDataPath, accountId);
            Directory.CreateDirectory(directory);
            _savePath = Path.Combine(directory, "stage_data.json");
        }

        public UniTask Save(StageSaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_savePath, json);
            WebGLFileSync.Sync();
            return UniTask.CompletedTask;
        }

        public UniTask<StageSaveData> Load()
        {
            if (!File.Exists(_savePath))
            {
                return UniTask.FromResult<StageSaveData>(null);
            }

            string json = File.ReadAllText(_savePath);
            return UniTask.FromResult(JsonUtility.FromJson<StageSaveData>(json));
        }
    }
}