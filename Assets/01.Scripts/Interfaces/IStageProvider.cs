namespace _01.Scripts.Interfaces
{
    public interface IStageProvider
    {
        int CurrentStage { get; }
        int CurrentKillCount { get; }
        int RequiredKillCount { get; }
        bool IsBossStage { get; }
    }
}