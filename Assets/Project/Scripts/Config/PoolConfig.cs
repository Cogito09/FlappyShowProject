using System.Collections.Generic;
using Cngine;
using UnityEngine;

namespace Flappy
{
    [CreateAssetMenu(fileName = "PoolConfig", menuName = "Configs/PoolConfig")]
    public class PoolConfig : BasePoolConfig
    {
        public Dictionary<FlappyPrefabType, int> PoolInitSpawnConfig;

        private Dictionary<int, int> _cachedConfig;
        private Dictionary<int, int> CachedConfig => _cachedConfig ??= ConvertConfig(PoolInitSpawnConfig);
        public override Dictionary<int, int> GetConfig()
        {
            return CachedConfig;
        }
    }
}