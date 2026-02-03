using System.IO;
using _01.Scripts.Core.Utils;
using _01.Scripts.Interfaces;
using UnityEngine;

namespace _01.Scripts.Ingame.Stage
{
    public class StageRepository
    {
        private readonly string _savePath;

        public StageRepository(string accountId)
        {
            string directory = Path.Combine(Application.persistentDataPath, accountId);
            Directory.CreateDirectory(directory);
            _savePath = Path.Combine(directory, "stage_data.json");
        }

        public void Save(StageSaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_savePath, json);
            WebGLFileSync.Sync();
        }

        public StageSaveData Load()
        {
            if (!File.Exists(_savePath))
            {
                return null;
            }

            string json = File.ReadAllText(_savePath);
            return JsonUtility.FromJson<StageSaveData>(json);
        }
    }
}