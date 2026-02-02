using System;
using System.Collections.Generic;

namespace _01.Scripts.Outgame.Account.Repo
{
    [Serializable]
    public class AccountSaveData
    {
        public string Id;
        public string Password;
    }

    [Serializable]
    public class AccountSaveDataCollection
    {
        public List<AccountSaveData> Entries = new List<AccountSaveData>();
    }
}