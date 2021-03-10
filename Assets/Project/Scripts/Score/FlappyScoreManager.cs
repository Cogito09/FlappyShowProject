using UnityEngine;

namespace Flappy
{
    public class FlappyScoreManager
    {
        public ScoreData CurrentScoreData;
        private int _bombCounter;
        
        public FlappyGameplayConfig.FlappyStageConfig CurrentStageConfig => _currentStageConfig ??= FlappyGameplayConfig.GetStageConfig(CurrentScoreData.CurrentStage);
        private FlappyGameplayConfig.FlappyStageConfig _currentStageConfig;
        private FlappyGameplayConfig _flappyGameplayConfig;
        private FlappyGameplayConfig FlappyGameplayConfig => _flappyGameplayConfig ??= MainConfig.FlappyGameplayConfig;

        private int? _maxNumberOfBombs;
        private int MaxNumberOfBombs => _maxNumberOfBombs ??= FlappyGameplayConfig.MaxNumberOfBombs;
        public bool IsAbleToUseBomb => CurrentScoreData.NumberOfBombs > 0;

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
                CurrentScoreData.CurrentStage++;
                EventManager.OnStageChanged?.Invoke();
            }

            EventManager.OnScoreChanged?.Invoke();
        }

        private void IncrementBombCounter()
        {
            if (CurrentScoreData.NumberOfBombs >= MaxNumberOfBombs)
            {
                return;
            }
            
            _bombCounter++;
            if (_bombCounter == FlappyGameplayConfig.AddBombScore)
            {
                _bombCounter = 0;
                CurrentScoreData.NumberOfBombs++;
                EventManager.OnBombsQuantityChanged?.Invoke();
            }
        }
        
        private void OnBombUsed()
        {
            CurrentScoreData.NumberOfBombs--;
            EventManager.OnBombsQuantityChanged?.Invoke();
        }
        
        private void OnStageChanged()
        {
            _currentStageConfig = null;
        }
        
        private void OnFlappyRoundReseted()
        {
            CurrentScoreData = new ScoreData();
            CurrentScoreData.CurrentStage = 1;
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
            return CurrentStageConfig.GetBackgroundColorOfCurrentScore(CurrentScoreData.Score);
        }
    }
}