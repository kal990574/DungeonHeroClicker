using System.IO;
using _01.Scripts.Interfaces;
using UnityEngine;

namespace _01.Scripts.Ingame.Stage
{
    public class StageRepository : IStageRepository
    {
        private readonly string _savePath;

        public StageRepository()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "stage_data.json");
        }

        public void Save(StageSaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_savePath, json);
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