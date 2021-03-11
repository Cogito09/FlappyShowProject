using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cngine
{
    public abstract class BasePrefabsConfig<T> : ScriptableObject where T : IBasePrefabTemplate
    {
        private List<IBasePrefabTemplate> _cachedTemplates;
        public List<IBasePrefabTemplate> CachedTemplates => _cachedTemplates ??= GetTemplates();

        protected abstract List<IBasePrefabTemplate> GetTemplates();

        public T1 InstantiatePrefab<T1>(int type) 
        {
            GameObject obj = null;
            for (int i = 0; i < CachedTemplates.Count; i++)
            {
                var template = CachedTemplates[i];
                if (template.GetPrefabTypeEnumConvertedToInt() != type)
                {
                    continue;
                }

                obj = template.GetPrefabTemplate();
            }

            var instantiatedObj = Instantiate(obj);
            var instantiatedObjSrcipt = instantiatedObj.GetComponent<T1>();
            return instantiatedObjSrcipt;
        }
    }
}