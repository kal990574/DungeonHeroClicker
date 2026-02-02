using System.Collections.Generic;
using System.IO;
using _01.Scripts.Core.Utils;
using _01.Scripts.Interfaces.Account;
using UnityEngine;

namespace _01.Scripts.Outgame.Account.Repo
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _savePath;
        private readonly Dictionary<string, AccountSaveData> _cache = new Dictionary<string, AccountSaveData>();

        public AccountRepository()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "account_data.json");
            LoadAll();
        }

        public bool Exists(string accountId)
        {
            return _cache.ContainsKey(accountId);
        }

        public AccountSaveData Load(string accountId)
        {
            _cache.TryGetValue(accountId, out AccountSaveData data);
            return data;
        }

        public void Save(AccountSaveData data)
        {
            _cache[data.Id] = data;
            SaveAll();
        }

        private void LoadAll()
        {
            if (!File.Exists(_savePath))
            {
                return;
            }

            string json = File.ReadAllText(_savePath);
            var collection = JsonUtility.FromJson<AccountSaveDataCollection>(json);

            if (collection?.Entries == null)
            {
                return;
            }

            foreach (AccountSaveData entry in collection.Entries)
            {
                _cache[entry.Id] = entry;
            }
        }

        private void SaveAll()
        {
            var collection = new AccountSaveDataCollection();

            foreach (AccountSaveData entry in _cache.Values)
            {
                collection.Entries.Add(entry);
            }

            string json = JsonUtility.ToJson(collection, true);
            File.WriteAllText(_savePath, json);
            WebGLFileSync.Sync();
        }
    }
}