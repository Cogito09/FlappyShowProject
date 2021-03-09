using UnityEngine;

namespace Flappy
{
    public class FlappyScoreManager
    {
        public ScoreData CurrentScoreData;

        private FlappyGameplayConfig _flappyGameplayConfig;
        private FlappyGameplayConfig FlappyGameplayConfig => _flappyGameplayConfig ??= MainConfig.FlappyGameplayConfig;
        private FlappyGameplayConfig.FlappyStageConfig CurrentStageConfig => _currentStageConfig ??= FlappyGameplayConfig.GetStageConfig(CurrentScoreData.CurrentStage);
        private FlappyGameplayConfig.FlappyStageConfig _currentStageConfig;

        private int _bombCounter;
        
        public void IncrementScore()
        {
            IncrementBombCounter();
            IncrementScoreCounter();
        }

        private void IncrementScoreCounter()
        {
            CurrentScoreData.Score++;
            if (CurrentStageConfig.IsWithInScoreRange(CurrentScoreData) == false)
            {
                EventManager.OnStageChanged?.Invoke();
            }

            EventManager.OnScoreChanged?.Invoke();
        }

        private void IncrementBombCounter()
        {
            _bombCounter++;
            if (_bombCounter == FlappyGameplayConfig.AddBombScore)
            {
                _bombCounter = 0;
                CurrentScoreData.NumberOfBombs++;
                EventManager.OnBombsQuantityChanged?.Invoke();
            }
        }

        private void OnStageChanged()
        {
            _currentStageConfig = null;
        }
        
        private void OnFlappyRoundReseted()
        {
            CurrentScoreData = new ScoreData();
            _bombCounter = 0;
            EventManager.OnStageChanged?.Invoke();
        }

        private void OnFlappyRoundFinished()
        {
            
        }

        private void OnFlappyRoundStarted()
        {

        }
        
        
        public FlappyScoreManager()
        {
            EventManager.OnFlappyRoundStarted += OnFlappyRoundStarted;
            EventManager.OnFlappyRoundFinished += OnFlappyRoundFinished;
            EventManager.OnFlappyRoundReseted += OnFlappyRoundReseted;
            EventManager.OnStageChanged += OnStageChanged;
        }


        ~FlappyScoreManager()
        {
            EventManager.OnFlappyRoundStarted -= OnFlappyRoundStarted;
            EventManager.OnFlappyRoundFinished -= OnFlappyRoundFinished;
            EventManager.OnFlappyRoundReseted -= OnFlappyRoundReseted;
            EventManager.OnStageChanged -= OnStageChanged;
        }
        
        
    }
}