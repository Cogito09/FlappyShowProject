using System;
using System.Collections.Generic;
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
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                FbLogin(null);
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
                FbLogin(null);
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
            FB.ShareLink( 
                new System.Uri("http://google.com"),
                "Check it out",
                $"Scored : {currentScore}. Can yo ubeat it? ", 
                new System.Uri("http://www.google.com") );
        }
    }
}
