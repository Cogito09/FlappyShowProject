namespace Flappy
{
    public class FlappyScoreManager
    {
        public ScoreData CurrentScoreData;

        private void OnFlappyRoundReseted()
        {
            CurrentScoreData = new ScoreData();
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
        }

        ~FlappyScoreManager()
        {
            EventManager.OnFlappyRoundStarted -= OnFlappyRoundStarted;
            EventManager.OnFlappyRoundFinished -= OnFlappyRoundFinished;
            EventManager.OnFlappyRoundReseted -= OnFlappyRoundReseted;
        }
        
        
    }
}