using System;
using Cngine;
using TMPro;
using UnityEngine;

namespace Flappy
{
    public class UIPopupScoreBehaviour : FlappyPopup
    {
        [SerializeField] private GameObject _highScoreAnimation;
        [SerializeField] private TextMeshProUGUI _currentScore;
        [SerializeField] private GameObject _fbButton;

        private FlappyScoreManager _flappyScoreManager;
        private FlappyScoreManager FlappyScoreManager => _flappyScoreManager ??= GameMaster.FlappyScoreManager;
        public bool IsCurrentScoreHighestScore => FlappyScoreManager.IsCurrentScoreHighestScore;
        public FlappyScoreData CurrentScore => FlappyScoreManager.CurrentFlappyScore;

        private bool IsAbleToShareOnFB => GameMaster.FacebookManager.IsAbleToShareOnFB;
        public override void Setup(object parameter)
        {
            base.Setup(parameter);

            SetupCurrentScore();
            SetupHighestScoreReached(IsCurrentScoreHighestScore);
            SetupFBButton();
        }

        private void SetupFBButton()
        {
            _fbButton.ChangeActive(IsAbleToShareOnFB);
        }

        private void SetupCurrentScore()
        {
            _currentScore.text = CurrentScore.ToString();
        }



        private void SetupHighestScoreReached(bool showHighestScore)
        {
            _highScoreAnimation.Deactivate();
            _highScoreAnimation.ChangeActive(showHighestScore);
        }
        
        public void OnClickRestart()
        {
            GameMaster.PopupManager.Hide(FlappyPopupType.ScorePopup);
            
            EventManager.OnFlappyRoundReseted?.Invoke();
            EventManager.OnFlappyRoundStarted?.Invoke();
        }
        
        public void OnClickShareOnFb()
        {
            GameMaster.FacebookManager.ShareScore(CurrentScore);
        }
        
    }
}