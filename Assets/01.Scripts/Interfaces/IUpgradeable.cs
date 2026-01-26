namespace _01.Scripts.Interfaces
{
    public interface IUpgradeable
    {
        public int CurrentLevel { get; set; }
        public int UpgradeCost { get; set; }
        public bool CanUpgrade { get; set; }
        
        void Upgrade();
    }
}