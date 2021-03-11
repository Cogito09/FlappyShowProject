using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cngine
{
    [CreateAssetMenu(fileName = "LoadingConfig", menuName = "Configs/LoadingConfig")]
    public class LoadingConfig : ScriptableObject
    {
        public GameObject[] SceneLoadersToBeSpawnedAtInit;
        public GameObject[] PrefabsToBeSpawnedAtInit;
        public GameObject[] UIPrefabsToBeSpawnedAtInit;
    }
}