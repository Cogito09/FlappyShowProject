using UnityEngine;

namespace Flappy
{
    public class UIPopupScoreBehaviour : FlappyPopup
    {
        public void OnClickRestart()
        {
            GameMaster.PopupManager.Hide(FlappyPopupType.ScorePopup);
            
            EventManager.OnFlappyRoundReseted?.Invoke();
            EventManager.OnFlappyRoundStarted?.Invoke();
        }
    }
}