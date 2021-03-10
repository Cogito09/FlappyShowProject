using System;
using Cngine;
using Cngine.PopupSystem;

namespace Flappy
{
    public class FlappyPopup : Popup 
    {
        public override void AnimateShow(Action onEnabled)
        {
            gameObject.ChangeActive(true);
            onEnabled?.Invoke();
        }

        public override void AnimateHide(Action onFinish)
        {
            gameObject.ChangeActive(false);
            onFinish?.Invoke();
        }

        public override void Setup(object parameter)
        {
        }

        public override void OnClickExit()
        {
            GameMaster.PopupManager.Hide((FlappyPopupType)_popupType);
                 }
    }
}