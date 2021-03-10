﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Cngine;
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
        public float FirstObstacleSpawnPositionX;
        public double DistanceBetweenObstacles;
        public float ObstacleRemoveXPosition;
        public Vector3 BridStartPosition;
        public float YPositionRandomizeUpperRange;
        public float YPositionRandomizeDownRange;
        public int AddBombScore;
        public int MaxNumberOfBombs;
        public float _bgYPosition;
        public double BombUseDoubleClickInterval;
        public double SightDistance;
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

            public Color GetBackgroundColorOfCurrentScore(int score)
            {
                // if (score < MinScoreRangeInclusive || score >= MaxScoreRangeExclusive)
                // {
                //     Log.Error("Current stage is not within the socre");
                //     EventManager.OnStageChanged?.Invoke();
                // }

                var stageScoreProgress = (float)(score - MinScoreRangeInclusive) / (MaxScoreRangeExclusive -MinScoreRangeInclusive);
                Log.Info($"stageScoreProgress : {stageScoreProgress}");
                return ColorRandomizeRange.Evaluate(stageScoreProgress);
            }
        }
        
        public FlappyObstaclesConfig.ObstacleConfig GetObstacleTypeByScore(ScoreData currentScoreData)
        {
            var cfg = FlappyStageConfigs.Find(config => config.IsWithInScoreRange(currentScoreData));
            if (cfg == null)
            {
                Log.Error("nie znajduje ostatniego configa");
                return null;
            }
            
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