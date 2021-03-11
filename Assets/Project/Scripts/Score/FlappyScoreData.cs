using System;

namespace Flappy
{
    [Serializable]
    public class FlappyScoreData : IComparable<FlappyScoreData>
    {        
        public int Score;
        public int CurrentStage;
        public int NumberOfBombs;
        
        public int CompareTo(FlappyScoreData other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }
            
            if (ReferenceEquals(null, other))
            {
                return 1;
            }

           
            if (Score > other.Score)
            {
                return - 1;
            }
            
            return 1;
        }
    }
}