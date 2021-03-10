using Cngine;
using Facebook.Unity;

namespace Flappy
{
    public class FacebookManager
    {
        public bool IsAbleToShareOnFB => IsInitialized();

        public bool IsInitialized()
        {
            if (FB.IsInitialized)
            {
                return true;
            }

            TryInitialize();
            return false;
        }

        public void TryInitialize()
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized == false)
                {
                    Log.Info("Failed to init Facebook");
                    return;
                }
            });
        }

        public void ShareScore(FlappyScoreData currentScore)
        {
        }
    }
}
