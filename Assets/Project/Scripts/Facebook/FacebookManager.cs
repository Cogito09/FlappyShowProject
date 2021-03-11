using System;
using System.Collections.Generic;
using Cngine;
using Facebook.Unity;

namespace Flappy
{
    public class FacebookManager
    {
        public bool IsAbleToShareOnFB => IsInitialized();
        public FacebookConfig FacebookConfig => MainConfig.FacebookConfig;
        
        
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
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                return;
            }
            
            FB.Init(() =>
            {
                if (FB.IsInitialized == false)
                {
                    Log.Info("Failed to init Facebook");
                    return;
                }
                
                FB.ActivateApp();
            });
        }

        public void FbLogin(Action onFinish)
        {
            FB.LogInWithReadPermissions(new List<string>() {"public_profile","email"}, callback =>
            {
                if (callback.Cancelled)
                {
                    Log.Error($" Log in failed,{callback.Error}");
                    onFinish?.Invoke();
                    return;
                }
                
                Log.Info($" Log in succed");
                onFinish?.Invoke();
            });
        }

        public void ShareScore(int currentScore)
        {
            FB.ShareLink(new System.Uri(FacebookConfig.ShareURL),
                FacebookConfig.GetShowScoreText(currentScore),
                FacebookConfig.GetShowScoreText(currentScore),
                new Uri(FacebookConfig.SharePhotoURL));

        }
    }
}
