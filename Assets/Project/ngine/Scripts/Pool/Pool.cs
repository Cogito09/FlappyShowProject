using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cngine
{
    public class Pool<T,T1>  where T : IBasePrefabTemplate where T1 : BasePrefabsConfig<T>
    {
        private T1 _prefabsConfig;

        private Queue<PoolObject> _pooledObjects;
        private Queue<PoolObject> PoolObjects => _pooledObjects ??= new Queue<PoolObject>();
        private int pooledObjectsType;

        public Pool(T1 basePrefabsConfig)
        {
            _prefabsConfig = basePrefabsConfig;
        }
        
        public void AddNewObjectsToPool(int goType,int capacity,Transform poolMarker)
        {
            pooledObjectsType = goType;
            for (int i = 0; i < capacity; i++)
            {
                var obj = _prefabsConfig.InstantiatePrefab<PoolObject>(goType);
                if (obj == null)
                {
                    Log.Error("instantiated obj is null, please check if given prefab original is present in config.");
                    continue;
                }

    
                obj.gameObject.SetActive(false);
                obj.OnPoolObjectCreated(goType,ReturnObject);
                PoolObjects.Enqueue(obj);
                
                if (poolMarker == null)
                {
                    Log.Error("poolMarker is null");
                    return;
                }
                
                obj.transform.SetSiblingIndex(poolMarker.GetSiblingIndex());
            }
        }
        
        public void RemoveObjectsFromPool(int goType,int capacity)
        {
            for (int i = PoolObjects.Count -1 ; i >= 0 ; i--)
            {
                if (PoolObjects.Peek().isActiveAndEnabled)
                {
                    break;
                }

                var obj = PoolObjects.Dequeue();
                Object.Destroy(obj);
            }
        }
        
        public PoolObject GetObject()
        {
            return PoolObjects.Dequeue();
        }

        public void ReturnObject(PoolObject obj)
        {
            PoolObjects.Enqueue(obj);
        }
        
        public IEnumerator AddNewObjectsToPoolAsync(int type, int numberOfObjectsToAdd, Transform poolMarker)
        {
            for (int i = 0; i < numberOfObjectsToAdd; i++)
            {
                var obj = _prefabsConfig.InstantiatePrefab<PoolObject>(type);
                if (obj == null)
                {
                    Debug.Log("instantiated obj is null");
                    continue;
                }

                obj.gameObject.SetActive(false);
                obj.OnPoolObjectCreated(type,ReturnObject);
                obj.transform.SetSiblingIndex(poolMarker.GetSiblingIndex());
                PoolObjects.Enqueue(obj);
                yield return null;
            }
        }
        
        public IEnumerator RemoveObjectsFromPoolAsync(int numberOfObjectsToRemove)
        {
            var initCount = PoolObjects.Count;
            for (int i = 0 ; i < numberOfObjectsToRemove ; i++)
            {
                if (PoolObjects.Count <= 0 || PoolObjects.Peek().isActiveAndEnabled)
                {
                    Log.Error($"Tried to dispose pool objects of type {pooledObjectsType.ToString()}, more than initialized. Initialized :{initCount}, Objets to be disposed :{numberOfObjectsToRemove}.");
                    break;
                }
                
                var obj = PoolObjects.Dequeue();
                Object.Destroy(obj);
                yield return null;
            }
        }
    }
}