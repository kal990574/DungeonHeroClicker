using System;
using System.Collections.Generic;

namespace _01.Scripts.Outgame.Upgrade.Repo
{
    [Serializable]
    public class UpgradeSaveData
    {
        public List<UpgradeStateEntry> Entries = new List<UpgradeStateEntry>();
    }

    [Serializable]
    public class UpgradeStateEntry
    {
        public string Id;
        public int Type;
        public int CurrentLevel;
        public bool IsPurchased;
    }
}