using _01.Scripts.Ingame.Stage;

namespace _01.Scripts.Interfaces
{
    public interface IStageRepository
    {
        void Save(StageSaveData data);
        StageSaveData Load();
    }
}