using _01.Scripts.Outgame.Upgrade.Repo;

namespace _01.Scripts.Interfaces.Upgrade
{
    public interface IUpgradeRepository
    {
        void Save(UpgradeSaveData data);
        UpgradeSaveData Load();
    }
}