using System;
using UnityEngine;

namespace Cngine
{
    public abstract class PoolObject : MonoBehaviour
    {
        [NonSerialized] private int Type;
        private Action<PoolObject> ReturnObjectToPool;
        public void OnPoolObjectCreated(int PrefabTypeConverted,Action<PoolObject> returnObjectToPoolAction)
        {
            Type = PrefabTypeConverted;
            ReturnObjectToPool = returnObjectToPoolAction;
        }

        protected abstract void StartRemovalAnimation();
        protected abstract void StartSpawnAnimation();

        public virtual void Remove(bool instant = false)
        {
            if (instant == false)
            {
                StartRemovalAnimation();
            }
            
            ReturnObjectToPool(this);
            gameObject.Deactivate();
        }

        public virtual void OnSpawned()
        {
            gameObject.Activate();
            StartSpawnAnimation();
        }
    }
}