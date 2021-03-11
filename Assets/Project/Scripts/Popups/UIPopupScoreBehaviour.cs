using System;
using System.Collections.Generic;
using Cngine;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

namespace Flappy
{
    public class UIPopupScoreBehaviour : FlappyPopup
    {
        [SerializeField] private UIScoreSlotBehaviour _prefabTemplate;
        [SerializeField] private List<UIScoreSlotBehaviour> _scores;
        [SerializeField] private GameObject _scoreContent;
        
        [SerializeField] private GameObject _highScoreAnimation;
        [SerializeField] private TextMeshProUGUI _currentScore;
        [SerializeField] private GameObject _fbButton;
        
        private FlappyScoreManager FlappyScoreManager => GameMaster.FlappyScoreManager;
        public bool IsCurrentScoreHighestScore => FlappyScoreManager.IsCurrentScoreHighestScore;
        public FlappyScoreData CurrentScore => FlappyScoreManager.CurrentFlappyScore;


        private bool IsAbleToShareOnFB => GameMaster.FacebookManager.IsAbleToShareOnFB;
        public override void Setup(object parameter)
        {
            base.Setup(parameter);

            SetupCurrentScore();
            SetupScoreBoard();
            SetupHighestScoreReached(IsCurrentScoreHighestScore);
            SetupFBButton();
        }

        private void SetupScoreBoard()
        {
            _scores.ForEach(_score => _score.gameObject.Deactivate());
            var bestScoresList = FlappyScoreManager.BestScores;
            for (int i = 0; i < bestScoresList.Count; i++)
            {
                if (_scores.Count <= i)
                {
                    _scores.Add(InstantiateNewSlot());
                }

                var score = bestScoresList[i];
                var scoreIndex = i + 1;
                _scores[i].gameObject.Activate();
                _scores[i].Setup(scoreIndex,score.Score);
            }
        }

        private UIScoreSlotBehaviour InstantiateNewSlot()
        {
            var slot = Instantiate(_prefabTemplate);
            slot.transform.SetParent(_scoreContent.transform);
            return slot;
        }

        private void SetupFBButton()
        {
            _fbButton.ChangeActive(IsAbleToShareOnFB);
        }

        private void SetupCurrentScore()
        {
            _currentScore.text = CurrentScore.Score.ToString();
        }

        private void SetupHighestScoreReached(bool showHighestScore)
        {
            _highScoreAnimation.Deactivate();
            _highScoreAnimation.ChangeActive(showHighestScore);
        }
        
        public void OnClickRestart()
        {
            if (FlappyManager.Instance.IsPlaying)
            {
                Log.Info("Tried to restart game whern it was playing");
                return;
            }
            
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