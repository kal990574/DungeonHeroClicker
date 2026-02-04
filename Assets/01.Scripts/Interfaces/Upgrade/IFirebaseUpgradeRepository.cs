using Cysharp.Threading.Tasks;
using _01.Scripts.Outgame.Upgrade.Repo;

namespace _01.Scripts.Interfaces.Upgrade
{
    public interface IFirebaseUpgradeRepository
    {
        UniTask Save(UpgradeSaveData data);
        UniTask<UpgradeSaveData> Load();
    }
}