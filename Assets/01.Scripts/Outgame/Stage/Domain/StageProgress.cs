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

        public StageProgress WithKillCountIncremented()
        {
            return new StageProgress(_currentStage, _currentKillCount + 1);
        }

        public StageProgress WithNextStage()
        {
            return new StageProgress(_currentStage + 1, 0);
        }

        public bool IsCleared(int requiredKillCount)
        {
            return _currentKillCount >= requiredKillCount;
        }

        public bool IsNextBoss(int requiredKillCount)
        {
            return _currentKillCount + 1 >= requiredKillCount;
        }
    }
}