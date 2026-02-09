using System.IO;
using Cysharp.Threading.Tasks;
using _01.Scripts.Core.Utils;
using _01.Scripts.Interfaces.Upgrade;
using _01.Scripts.Outgame.Account.Manager;
using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade.Repo
{
    public class UpgradeRepository : IUpgradeRepository
    {
        private readonly string _savePath;

        public UpgradeRepository()
        {
            string accountId = AccountManager.Instance.CurrentAccountId;
            string directory = Path.Combine(Application.persistentDataPath, accountId);
            Directory.CreateDirectory(directory);
            _savePath = Path.Combine(directory, "upgrade_data.json");
        }

        public UniTask Save(UpgradeSaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_savePath, json);
            WebGLFileSync.Sync();
            return UniTask.CompletedTask;
        }

        public UniTask<UpgradeSaveData> Load()
        {
            if (!File.Exists(_savePath))
            {
                return UniTask.FromResult<UpgradeSaveData>(null);
            }

            string json = File.ReadAllText(_savePath);
            return UniTask.FromResult(JsonUtility.FromJson<UpgradeSaveData>(json));
        }
    }
}