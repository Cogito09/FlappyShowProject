using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

namespace Cngine
{

    public abstract  class BasePoolConfig : SerializedScriptableObject
    {
        public Dictionary<int, int> EnumTypeToNumberOfObjectsBaseConfig => GetConfig();
        public abstract Dictionary<int, int> GetConfig();
        

        protected Dictionary<int, int> ConvertConfig<T>(Dictionary<T, int> config) where T : Enum
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            config.ForEach(pair => {  dict.Add(Convert.ToInt32(pair.Key), pair.Value); });
            return dict;
        }

    }
}