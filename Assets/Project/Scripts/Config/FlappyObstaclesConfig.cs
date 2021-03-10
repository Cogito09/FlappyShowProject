using System;
using System.Collections.Generic;
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
                return ColorRange.Evaluate(UnityEngine.Random.Range(0, 1));
            }
        }
        
        public ObstacleConfig GetObstacleConfig(int id)
        {
            return Obstacles.Find(config => config.Id == id);
        }
    }
}
