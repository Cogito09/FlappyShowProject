using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cngine
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class Popup : MonoBehaviour, IPopup
    {
        private object _lastParameter;
        protected object _popupType;
        public bool HaveExitButton;
        public bool ShowBackground;
        public bool IsNotAbleToCloseWithBackButton;
        
        protected Canvas _canvas;
        protected Canvas Canvas => _canvas ? _canvas : (_canvas = GetComponent<Canvas>());
        private RectTransform _rect;
        protected RectTransform RectTransform => _rect ? _rect : (_rect = GetComponent<RectTransform>());
        protected Button ExitButton;
        public bool IsVisible { get; set; }

        public void SetPopupType(object type)
        {
            _popupType = type;
        }
        

        public abstract void AnimateShow(Action onEnabled);
        public abstract void AnimateHide(Action onFinish);
        public abstract void Setup(object parameter);
        public abstract void OnClickExit();
        
        public void SetParameter(object param)
        {
            _lastParameter = param;
        }
        
        public virtual void SetDepth(int depth)
        {
            if (Canvas == null)
            {
                Log.Error("Popup doesn't have canvas");
                return;
            }

            Canvas.SetDepth(depth);
        }
    }
}