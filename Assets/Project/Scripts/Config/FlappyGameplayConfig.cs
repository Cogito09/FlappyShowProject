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
        public float ObstacleSpawnDistanceFromCenter;
        public double DistanceBetweenObstacles;
        public float ObstacleRemoveXPosition;
        public Vector3 BridStartPosition;
        public float YPositionRandomizeUpperRange;
        public float YPositionRandomizeDownRange;
        public int AddBombScore;
        public int MaxNumberOfBombs;
        public float _bgYPosition;
        
        public List<FlappyStageConfig> FlappyStageConfigs;


        [Serializable]
        public class FlappyStageConfig
        {
            public int Id;
            public int MinScoreRangeInclusive;
            public int MaxScoreRangeExclusive;
            public bool IsMaxScore;

            [Obstacle] public int ObstacleType;
            public Gradient ColorRandomizeRange;

            public bool IsWithInScoreRange(ScoreData currentScoreData)
            {
                var scoreValue = currentScoreData.Score;
                if (IsMaxScore)
                {
                    return scoreValue >= MinScoreRangeInclusive;
                }
                
                return IsMaxScore ? scoreValue >= MinScoreRangeInclusive :
                    (scoreValue >= MinScoreRangeInclusive && scoreValue < MaxScoreRangeExclusive);
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
            return UnityEngine.Random.Range(YPositionRandomizeUpperRange, YPositionRandomizeDownRange);
        }

        public FlappyStageConfig GetStageConfig(int currentStage)
        {           
            //FlappyStageConfigs.Find(config => config.Id == currentStage));

            for (int i = 0; i < FlappyStageConfigs.Count; i++)
            {
                if (FlappyStageConfigs[i].Id != currentStage)
                {
                    continue;
                }

                return FlappyStageConfigs[i];
            }
            
            return FlappyStageConfigs[FlappyStageConfigs.Count -1];
        }
    }
}