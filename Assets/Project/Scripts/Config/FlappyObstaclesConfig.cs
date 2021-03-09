using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Flappy
{
    [CreateAssetMenu(fileName = "FlappyObstaclesConfig", menuName = "Configs/FlappyObstaclesConfig", order = 0)]
    public class FlappyObstaclesConfig : ScriptableObject
    {
        [Serializable]
        public class ObstacleConfig
        {
            public int Id;
            public string DevName;
            public Sprite Sprite;
            public Gradient ColorRange;

            public Color RandomizeColor()
            {
                return ColorRange.Evaluate(UnityEngine.Random.Range(0, 1));
            }
        }

        
        public List<ObstacleConfig> Obstacles;

        public ObstacleConfig GetObstacleConfig(int id)
        {
            return Obstacles.Find(config => config.Id == id);
        }
    }
}
