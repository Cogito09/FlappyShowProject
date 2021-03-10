using System.Collections.Generic;
using Cngine;
using Cngine.PopupSystem;
using UnityEngine;

namespace Flappy
{
    public class FlappyScoreManager
    {
        private List<FlappyScoreData> _bestScores;
        public List<FlappyScoreData> BestScores => _bestScores ??= GameMaster.Save.BestScoresSave;
        private void SaveScore()
        {
            GameMaster.Save.BestScoresSave = _bestScores;
        }
        
        public FlappyScoreData CurrentFlappyScore;
        private int _bombCounter;

        public bool IsCurrentScoreHighestScore => BestScores.Count <= 0 ? false : BestScores[0].Score == CurrentFlappyScore.Score;
        public FlappyGameplayConfig.FlappyStageConfig CurrentStageConfig => _currentStageConfig ??= FlappyGameplayConfig.GetStageConfig(CurrentFlappyScore.CurrentStage);
        private FlappyGameplayConfig.FlappyStageConfig _currentStageConfig;
        private FlappyGameplayConfig _flappyGameplayConfig;
        private FlappyGameplayConfig FlappyGameplayConfig => _flappyGameplayConfig ??= MainConfig.FlappyGameplayConfig;
        public int MaxNumberOfStoredSaves => MainConfig.FlappyGameplayConfig.MaxNumberOfStoredScoreSaves;
        private int? _maxNumberOfBombs;
        private int MaxNumberOfBombs => _maxNumberOfBombs ??= FlappyGameplayConfig.MaxNumberOfBombs;
        public bool IsAbleToUseBomb => CurrentFlappyScore.NumberOfBombs > 0;

        public void IncrementScore()
        {
            IncrementBombCounter();
            IncrementScoreCounter();
        }

        private void IncrementScoreCounter()
        {
            CurrentFlappyScore.Score++;
            if (CurrentStageConfig.IsWithInScoreRange(CurrentFlappyScore) == false)
            {
                CurrentFlappyScore.CurrentStage++;
                EventManager.OnStageChanged?.Invoke();
            }

            EventManager.OnScoreChanged?.Invoke();
        }

        private void IncrementBombCounter()
        {
            if (CurrentFlappyScore.NumberOfBombs >= MaxNumberOfBombs)
            {
                return;
            }
            
            _bombCounter++;
            if (_bombCounter == FlappyGameplayConfig.AddBombScore)
            {
                _bombCounter = 0;
                CurrentFlappyScore.NumberOfBombs++;
                EventManager.OnBombsQuantityChanged?.Invoke();
            }
        }
        
        private void OnBombUsed()
        {
            CurrentFlappyScore.NumberOfBombs--;
            EventManager.OnBombsQuantityChanged?.Invoke();
        }
        
        private void OnStageChanged()
        {
            _currentStageConfig = null;
        }
        
        private void OnFlappyRoundReseted()
        {
            CurrentFlappyScore = new FlappyScoreData();
            CurrentFlappyScore.CurrentStage = 1;
            _bombCounter = 0;
            EventManager.OnStageChanged?.Invoke();
        }

        private void OnFlappyRoundFinished()
        {
            ValidateScore();
        }
        
        private void ValidateScore()
        {
            var currentScore = CurrentFlappyScore;
            var scores = BestScores;
            scores.Add(currentScore);
            scores.Sort();
            
            _bestScores = TrimScoreListToBest(scores);
            SaveScore();
            
            GameMaster.PopupManager.Show(FlappyPopupType.ScorePopup);
        }

        private List<FlappyScoreData> TrimScoreListToBest(List<FlappyScoreData> scores)
        {
            if (scores.Count -1  > MaxNumberOfStoredSaves)
            {
                for (int i = scores.Count - 1; i >= MaxNumberOfStoredSaves; i--)
                {
                    scores.RemoveAt(i);
                }
            }

            return scores;
        }

        private void OnFlappyRoundStarted()
        {

        }
        
        public FlappyScoreManager()
        {
            EventManager.OnFlappyRoundStarted += OnFlappyRoundStarted;
            EventManager.OnFlappyRoundFinished += OnFlappyRoundFinished;
            EventManager.OnFlappyRoundReseted += OnFlappyRoundReseted;
            EventManager.OnBombUsed += OnBombUsed;
            EventManager.OnStageChanged += OnStageChanged;
        }
        
        ~FlappyScoreManager()
        {
            EventManager.OnFlappyRoundStarted -= OnFlappyRoundStarted;
            EventManager.OnFlappyRoundFinished -= OnFlappyRoundFinished;
            EventManager.OnFlappyRoundReseted -= OnFlappyRoundReseted;
            EventManager.OnStageChanged -= OnStageChanged;
            EventManager.OnBombUsed -= OnBombUsed;
        }

        public Color GetBackgroundColorOfCurrentScore()
        {
            return CurrentStageConfig.GetBackgroundColorOfCurrentScore(CurrentFlappyScore.Score);
        }
    }
}