using System;
using System.Collections.Generic;
using Cngine;
using UnityEngine;

namespace Flappy
{
    [CreateAssetMenu(fileName = "FlappyPrefabsConfig", menuName = "Configs/FlappyPrefabsConfig", order = 0)]
    public class FlappyPrefabsConfig : BasePrefabsConfig<FlappyPrefabTemplate>
    {
        public List<FlappyPrefabTemplate> Templates;
        protected override List<IBasePrefabTemplate> GetTemplates()
        {
            return new List<IBasePrefabTemplate>(Templates);
        }
    }
    
    public class FlappyPrefabTemplate : IBasePrefabTemplate
    {
        public FlappyPrefabType PrefabType;
        public GameObject PrefabObject;
        
        public int GetPrefabTypeEnumConvertedToInt()
        {
            return Convert.ToInt32(PrefabType);
        }

        public GameObject GetPrefabTemplate()
        {
            return PrefabObject;
        }
    }

    public enum FlappyPrefabType
    {
        Unknown = 0,
        Obstacle = 1,
        ObstacleRemovalAnimation = 2,
        BackgroundTile = 3
    }
}