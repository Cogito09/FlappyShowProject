using System;

namespace Cngine
{
    public interface IPopup
    {
        bool IsVisible { get; set; }
        void AnimateShow(Action onEnabled);
        void AnimateHide(Action onFinish);
        void Setup(object parameter);
        void OnClickExit();
        void SetDepth(int depth);
        void SetPopupType(object type);
    }
}