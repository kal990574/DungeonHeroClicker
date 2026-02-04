using Cysharp.Threading.Tasks;
using _01.Scripts.Outgame.Stage.Repo;

namespace _01.Scripts.Interfaces
{
    public interface IFirebaseStageRepository
    {
        UniTask Save(StageSaveData data);
        UniTask<StageSaveData> Load();
    }
}