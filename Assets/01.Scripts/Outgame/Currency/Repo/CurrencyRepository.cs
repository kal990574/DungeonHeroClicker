using System.IO;
using _01.Scripts.Interfaces.Currency;
using UnityEngine;

namespace _01.Scripts.Outgame.Currency.Repo
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly string _savePath;

        public CurrencyRepository()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "currency_data.json");
        }

        public void Save(CurrencySaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_savePath, json);
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