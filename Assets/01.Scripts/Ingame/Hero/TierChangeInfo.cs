namespace _01.Scripts.Ingame.Hero
{
    public readonly struct TierChangeInfo
    {
        public string PreviousTierName { get; }
        public string NewTierName { get; }
        public int NewTierLevel { get; }

        public TierChangeInfo(string previousTierName, string newTierName, int newTierLevel)
        {
            PreviousTierName = previousTierName;
            NewTierName = newTierName;
            NewTierLevel = newTierLevel;
        }
    }
}