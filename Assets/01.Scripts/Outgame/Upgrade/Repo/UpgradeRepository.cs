using System.IO;
using _01.Scripts.Interfaces.Upgrade;
using UnityEngine;

namespace _01.Scripts.Outgame.Upgrade.Repo
{
    public class UpgradeRepository : IUpgradeRepository
    {
        private readonly string _savePath;

        public UpgradeRepository()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "upgrade_data.json");
        }

        public void Save(UpgradeSaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_savePath, json);
        }

        public UpgradeSaveData Load()
        {
            if (!File.Exists(_savePath))
            {
                return null;
            }

            string json = File.ReadAllText(_savePath);
            return JsonUtility.FromJson<UpgradeSaveData>(json);
        }
    }
}