#if USE_FIREBASE
using System;
using Cngine;
using Firebase.Auth;

namespace Cngine
{
    public class Auth
    {
        private FirebaseAuth _auth;
        public FirebaseUser User { get; private set; }

        public void Initialize()
        {
            _auth = FirebaseAuth.DefaultInstance;
            _auth.StateChanged += AuthOnStateChanged;
        }

        private void AuthOnStateChanged(object sender, EventArgs e)
        {
            if (_auth.CurrentUser == User)
            {
                return;
            }

            bool signedIn = User != _auth.CurrentUser && _auth.CurrentUser != null;
            if (signedIn == false && User != null)
            {
                Log.Info("Signed out " + User.UserId);
            }

            User = _auth.CurrentUser;
            if (signedIn)
            {
                Log.Info("Signed in " + User.UserId);
            }
        }

        public void Login(Action<bool> onFinish)
        {
            _auth.SignInAnonymouslyAsync().ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Log.Info($"{task.Exception}");
                    Log.Error($"Login task was canceled or faulted ( {task.IsCanceled} | {task.IsFaulted})");
                    UnityMainThreadDispatcher.Instance().Enqueue(() => onFinish(false));
                    return;
                }

                User = task.Result;
                Log.Info($"User signed in succesfuly {User.DisplayName}, {User.UserId}");
                UnityMainThreadDispatcher.Instance().Enqueue(() => onFinish(User != null));
            });
        }
    }
}
#endif