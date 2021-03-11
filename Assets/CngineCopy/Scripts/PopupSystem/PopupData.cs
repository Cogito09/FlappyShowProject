using System;

namespace Cngine.PopupSystem
{
    [Serializable]
    public class PopupData<T> where T : struct, IConvertible
    {
        public IPopup Popup;
        public int Depth;
        public object Data;
        public T PopupType;
    }
}