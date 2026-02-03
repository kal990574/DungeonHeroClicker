using System.IO;
using _01.Scripts.Core.Utils;
using _01.Scripts.Interfaces.Currency;
using UnityEngine;

namespace _01.Scripts.Outgame.Currency.Repo
{
    public class CurrencyRepository
    {
        private readonly string _savePath;

        public CurrencyRepository(string accountId)
        {
            string directory = Path.Combine(Application.persistentDataPath, accountId);
            Directory.CreateDirectory(directory);
            _savePath = Path.Combine(directory, "currency_data.json");
        }

        public void Save(CurrencySaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_savePath, json);
            WebGLFileSync.Sync();
        }

        public CurrencySaveData Load()
        {
            if (!File.Exists(_savePath))
            {
                return null;
            }

            string json = File.ReadAllText(_savePath);
            return JsonUtility.FromJson<CurrencySaveData>(json);
        }
    }
}