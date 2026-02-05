using System;

namespace _01.Scripts.Outgame.Stage.Domain
{
    public class StageProgress
    {
        private readonly int _currentStage;
        private readonly int _currentKillCount;

        public int CurrentStage => _currentStage;
        public int CurrentKillCount => _currentKillCount;

        public StageProgress(int currentStage = 1, int currentKillCount = 0)
        {
            if (currentStage < 1)
            {
                throw new ArgumentException("스테이지는 1 이상이어야 합니다.");
            }

            if (currentKillCount < 0)
            {
                throw new ArgumentException("킬 카운트는 0 이상이어야 합니다.");
            }

            _currentStage = currentStage;
            _currentKillCount = currentKillCount;
        }
    }
}