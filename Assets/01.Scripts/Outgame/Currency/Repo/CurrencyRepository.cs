using System.IO;
using Cysharp.Threading.Tasks;
using _01.Scripts.Core.Utils;
using _01.Scripts.Interfaces.Currency;
using _01.Scripts.Outgame.Account.Manager;
using UnityEngine;

namespace _01.Scripts.Outgame.Currency.Repo
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly string _savePath;

        public CurrencyRepository()
        {
            string accountId = AccountManager.Instance.CurrentAccountId;
            string directory = Path.Combine(Application.persistentDataPath, accountId);
            Directory.CreateDirectory(directory);
            _savePath = Path.Combine(directory, "currency_data.json");
        }

        public UniTask Save(CurrencySaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_savePath, json);
            WebGLFileSync.Sync();
            return UniTask.CompletedTask;
        }

        public UniTask<CurrencySaveData> Load()
        {
            if (!File.Exists(_savePath))
            {
                return UniTask.FromResult<CurrencySaveData>(null);
            }

            string json = File.ReadAllText(_savePath);
            return UniTask.FromResult(JsonUtility.FromJson<CurrencySaveData>(json));
        }
    }
}