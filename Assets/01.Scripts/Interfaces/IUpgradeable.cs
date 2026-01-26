namespace _01.Scripts.Interfaces
{
    public interface IUpgradeable
    {
        public int CurrentLevel { get; }
        public int UpgradeCost { get; }
        public bool CanUpgrade { get; }
        
        void DoUpgrade();
    }
}