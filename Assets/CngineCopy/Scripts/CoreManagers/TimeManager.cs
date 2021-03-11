using Unity.Mathematics;
using UnityEngine;

namespace Cngine
{
    public class TimeManager
    {
        private const double FullCycleTime = 2 * math.PI;
        public double TimeBeforeTrackStart = 3f;
        private double CurrentPhaseTimeTime;
        private float TrackTimeStartTimestamp;

        public double GetPhaseTime()
        {
            CurrentPhaseTimeTime = CurrentPhaseTimeTime + Time.deltaTime;
            CurrentPhaseTimeTime = CurrentPhaseTimeTime < FullCycleTime ? CurrentPhaseTimeTime : CurrentPhaseTimeTime - FullCycleTime;
            return CurrentPhaseTimeTime;
        }

        public void RegisterTrackTimeStart()
        {
            TrackTimeStartTimestamp = Time.time + (float)TimeBeforeTrackStart;
        }

        public double GetCurrentTrackTimeTime()
        {
            return Time.time - TrackTimeStartTimestamp;
        }
    }
}