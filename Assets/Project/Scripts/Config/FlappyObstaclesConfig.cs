using System;
using System.Collections.Generic;
using Cngine;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace Flappy
{
    [CreateAssetMenu(fileName = "FlappyObstaclesConfig", menuName = "Configs/FlappyObstaclesConfig", order = 0)]
    public class FlappyObstaclesConfig : ScriptableObject
    {
        public List<ObstacleConfig> Obstacles;
        
        [Serializable]
        public class ObstacleConfig
        {
            public int Id;
            public string DevName;
            public Gradient ColorRange;
            [PreviewField] public Sprite Sprite;
            
            public Color RandomizeColor()
            {
                var colorRandomization = UnityEngine.Random.Range(0f, 1f);
                Log.Info($"stickColorRandomizationValue : {colorRandomization}");
                return ColorRange.Evaluate(colorRandomization);
            }
        }
        
        public ObstacleConfig GetObstacleConfig(int id)
        {
            return Obstacles.Find(config => config.Id == id);
        }
    }
}
