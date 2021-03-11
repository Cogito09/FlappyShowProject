using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cngine
{
    public class PoolManager<T,T1> where T : IBasePrefabTemplate where T1 : BasePrefabsConfig<T>
    {
        private T1 PrefabsConfig;
        
        private const int DEFAULT_POOL_CAPACITY = 100;
        private Dictionary<int, Pool<T,T1>> _pools;
        private Dictionary<int, Pool<T,T1>> Pools => _pools ??= new Dictionary<int, Pool<T,T1>>();

        public PoolManager(T1 prefabsConfig)
        {
            PrefabsConfig = prefabsConfig;
        }
        
        private Pool<T,T1> CreateNewPool()
        {
            return new Pool<T,T1>(PrefabsConfig);
        }
        
        public void InitNewPoolObjects(Transform poolMarker,BasePoolConfig poolConfig)
        {
            if (poolMarker == null)
            { 
                Log.Error("poolMarker object not set");
                return;
            }
            
            var poolCfg = poolConfig.GetConfig();
            foreach (var kV in poolCfg)
            {
                int objType = (int)kV.Key;
                CreatePool(objType, kV.Value,poolMarker);
            }
        }
        
        public IEnumerator InitNewPoolObjectsAsync(Transform poolMarker,BasePoolConfig poolConfig)
        {
            if (poolMarker == null)
            { 
                Log.Error("poolMarker object not set");
                yield return null;
            }
            
            var poolCfg = poolConfig.GetConfig();
            foreach (var kV in poolCfg)
            {
                int objType = (int)kV.Key;
                yield return GameMasterBase.BaseInstance.StartCoroutine(CreatePoolAsync(objType, kV.Value,poolMarker));
            }
        }
        
        public void DisposePoolObjects(BasePoolConfig config)
        { 
            var poolCfg = config.GetConfig();
            foreach (var kV in poolCfg)
            {
                int objType = (int)kV.Key;
                var numberOfObjects = kV.Value;
                Pools[objType]?.RemoveObjectsFromPool(objType, numberOfObjects);
            }
        }
        
        public IEnumerator DisposePoolObjectsAsync(BasePoolConfig config)
        {
            var poolCfg = config.GetConfig();
            foreach (var kV in poolCfg)
            {
                int objType = (int)kV.Key;
                var numberOfObjects = kV.Value;
                yield return GameMasterBase.BaseInstance.StartCoroutine((Pools[objType]?.RemoveObjectsFromPoolAsync(numberOfObjects)));
            }

            yield return null;
        }
        
        private void CreatePool(int type ,int capacity ,Transform poolMarker)
        {
            if (Pools.ContainsKey(type))
            {
                Pools[type].AddNewObjectsToPool(type, capacity, poolMarker);
                Debug.Log("Pool of that key already exists");
                return;
            }

            Pools[type] = CreateNewPool();
            Pools[type].AddNewObjectsToPool(type, capacity,poolMarker);
        }
        
        private IEnumerator CreatePoolAsync(int type ,int capacity ,Transform poolMarker)
        {
            if (Pools.ContainsKey(type))
            {
                yield return GameMasterBase.BaseInstance.StartCoroutine(Pools[type].AddNewObjectsToPoolAsync(type, capacity, poolMarker));
                Debug.Log("Pool of that key already exists");
            }

            Pools[type] = CreateNewPool();
            yield return GameMasterBase.BaseInstance.StartCoroutine(Pools[type].AddNewObjectsToPoolAsync(type, capacity,poolMarker));
        }
        
        public PoolObject SpawnObject<T1>(T1 typeEnum) where T1 : Enum
        {
            var type = Convert.ToInt32(typeEnum);
            
            if (Pools.ContainsKey(type) == false)
            {
                Debug.Log("Object pool doesnt exist, creating new pool");
                CreatePool(type, DEFAULT_POOL_CAPACITY,null);
            }

            var poolObj = Pools[type].GetObject();
            if (poolObj == null)
            {
                Debug.Log("Object not obtained from pull, passing null value");
                return null;
            }

            poolObj.OnSpawned();
            return poolObj;
        }

        public void ReturnObject<T1>(int type,T1 obj) where T1 : PoolObject
        {
            if (Pools.ContainsKey(type) == false)
            {
                Debug.Log("Object pool doesnt exist, creating new pool");
                CreatePool(type, DEFAULT_POOL_CAPACITY,null);
            }

            Pools[type].ReturnObject(obj);
        }
    }
}