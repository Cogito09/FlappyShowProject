using System;
using Cngine;
using UnityEngine;

namespace Cngine
{
    public abstract class BaseMainConfig : ScriptableObject
    {
        public abstract LoadingConfig GetLoadingConfig();
        public abstract BasePopupConfig GetPopupConfig();
    }
}