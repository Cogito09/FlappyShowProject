#if USE_FIREBASE
using System;
using Cngine;
using Firebase;
using Firebase.Database;
using ProjectOne;
using UnityEngine;

namespace Cngine
{
    public class FirebaseManager : MonoBehaviour , ISpawnCallbackReceiver
    {
        public Auth Auth;
        public FirebaseApp _firebaseApp;

        public FirebaseDatabase _firebaseDatabase;

        
        public void Initialize(Action<bool> onFinish)
        {
            _firebaseApp = FirebaseApp.DefaultInstance;
            Auth = new Auth();

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                Log.Info("Firebase app have checked it's dependencies.");
                var dependencyStatus = task.Result;
                if (dependencyStatus != Firebase.DependencyStatus.Available)
                {
                    Log.Warning("Firebase is not safe to use skipping firebase");
                    return;
                }

                _firebaseDatabase = FirebaseDatabase.DefaultInstance;
                Auth.Initialize();

                onFinish?.Invoke(true);
            });


            onFinish?.Invoke(false);
        }


        public DatabaseReference GetDatabaseReference(string path)
        {
            return _firebaseDatabase.GetReference(path);
        }

        public void OnInstantiated()
        {
            GameMasterBase.FirebaseManager = this;
            GameMasterBase.FirebaseManager.Initialize(initResult =>
            {
                if (initResult == false)
                {
                    // show unable to login popup
                    return;
                }

            });
        }
    }
}
#endif
