using _01.Scripts.Outgame.Stage.Repo;

namespace _01.Scripts.Interfaces
{
    public interface IStageRepository
    {
        void Save(StageSaveData data);
        StageSaveData Load();
    }
}