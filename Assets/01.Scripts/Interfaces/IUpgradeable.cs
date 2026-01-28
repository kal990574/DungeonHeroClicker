using _01.Scripts.Core.Utils;

namespace _01.Scripts.Interfaces
{
    public interface IUpgradeable
    {
        public int CurrentLevel { get; }
        public BigNumber UpgradeCost { get; }
        public bool CanUpgrade { get; }

        void DoUpgrade();
    }
}