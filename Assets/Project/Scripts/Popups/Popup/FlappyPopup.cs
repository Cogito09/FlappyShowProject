﻿using System;
using Cngine;
using Cngine.PopupSystem;

namespace Flappy
{
    public class FlappyPopup : Popup 
    {
        public override void AnimateShow(Action onEnabled)
        {

        }

        public override void AnimateHide(Action onFinish)
        {

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