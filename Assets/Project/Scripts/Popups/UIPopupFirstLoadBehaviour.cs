using UnityEngine;

namespace Flappy
{
    public class UIPopupFirstLoadBehaviour : FlappyPopup
    {
        public void OnClickPlay()
        {
            GameMaster.PopupManager.Hide(FlappyPopupType.FirstLoadPopup);
            EventManager.OnFlappyRoundStarted?.Invoke();
        }
    }
}