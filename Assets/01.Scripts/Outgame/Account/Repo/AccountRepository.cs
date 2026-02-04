using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using _01.Scripts.Core.Utils;
using _01.Scripts.Interfaces.Account;
using _01.Scripts.Outgame.Account.Domain;
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

        public UniTask<AccountResult> Register(string email, string password)
        {
            if (_cache.ContainsKey(email))
            {
                return UniTask.FromResult(AccountResult.Fail("This ID is already taken."));
            }

            var saveData = new AccountSaveData
            {
                Id = email,
                Password = password
            };

            _cache[email] = saveData;
            SaveAll();

            return UniTask.FromResult(AccountResult.Ok());
        }

        public UniTask<AccountResult> Login(string email, string password)
        {
            if (!_cache.TryGetValue(email, out AccountSaveData data))
            {
                return UniTask.FromResult(AccountResult.Fail("Invalid ID or password."));
            }

            if (data.Password != password)
            {
                return UniTask.FromResult(AccountResult.Fail("Invalid ID or password."));
            }

            return UniTask.FromResult(AccountResult.Ok(email));
        }

        public void Logout()
        {
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