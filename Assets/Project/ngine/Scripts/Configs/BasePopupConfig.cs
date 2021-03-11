using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cngine
{
    public abstract class BasePopupConfig : ScriptableObject
    {
        public abstract List<IPopupPrefab> GetPopupsToLoad();
    }

    public interface IPopupPrefab
    {
        int GetPopupType();
        MonoBehaviour GetPrefab();
        bool GetIfIsStaticField();
    }
    
}