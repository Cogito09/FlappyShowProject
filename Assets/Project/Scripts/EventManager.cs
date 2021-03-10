using System;

namespace Flappy
{
    public static class EventManager
    {
        public static Action OnFlappyRoundFinished;
        public static Action OnFlappyRoundReseted;
        public static Action OnFlappyRoundStarted;
        public static Action OnFlappyObstacleHit;
        public static Action OnStageChanged;
        public static Action OnScoreChanged;
        public static Action OnBombsQuantityChanged;
        public static Action OnBombUsed;
    }
}