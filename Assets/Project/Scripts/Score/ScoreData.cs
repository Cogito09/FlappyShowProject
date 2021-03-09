using System;

namespace Flappy
{
    [Serializable]
    public struct ScoreData
    {
        public int CurrentStage;
        public int Score;
        public int NumberOfBombs;
    }
}