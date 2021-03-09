using System;
using System.Collections.Generic;
using Flappy.Editor;
using UnityEngine;
using Random = System.Random;

namespace Flappy
{
    [CreateAssetMenu(fileName = "FlappyGameplayConfig", menuName = "Configs/FlappyGameplayConfig", order = 0)]
    public class FlappyGameplayConfig : ScriptableObject
    {

        public float WorldMovementSpeed;
        public float MapEdgePosition;
        public int TotalNumberOfVisibleTiles;
        public float FirstObstacleSpawnDistance;
        public double DistanceBetweenObstacles;
        public float ObstacleRemoveXPosition;
            
        public float YPositionRandomizeRange;
        
        public List<FlappyStageConfig> FlappyStageConfigs;
        [Serializable]
        public class FlappyStageConfig
        {
            public int MinScoreRangeInclusive;
            public int MaxScoreRangeExclusive;

            [Obstacle] public int ObstacleType;
            public Gradient ColorRandomizeRange;

            public bool IsWithInScoreRange(ScoreData currentScoreData)
            {
                var scoreValue = currentScoreData.Score;
                return (scoreValue >= MinScoreRangeInclusive && scoreValue < MaxScoreRangeExclusive);
            }
        }
        
        public FlappyObstaclesConfig.ObstacleConfig GetObstacleTypeByScore(ScoreData currentScoreData)
        {
            var cfg = FlappyStageConfigs.Find(config => config.IsWithInScoreRange(currentScoreData));
            var obstacleCfg = MainConfig.FlappyObstaclesConfig.GetObstacleConfig(cfg.ObstacleType);
            return obstacleCfg;
        }

        public float RandomizeYPositionForObstacle()
        {
            return UnityEngine.Random.Range(-YPositionRandomizeRange, YPositionRandomizeRange);
        }
    }
}