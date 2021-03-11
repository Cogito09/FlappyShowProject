using System;
using UnityEngine;

namespace Cngine
{
    public interface IBasePrefabTemplate
    {
        public int GetPrefabTypeEnumConvertedToInt();
        public GameObject GetPrefabTemplate();
    }
}