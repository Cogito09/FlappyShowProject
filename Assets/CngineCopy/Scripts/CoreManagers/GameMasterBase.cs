using System;
using System.Collections;
using UnityEngine;

namespace Cngine
{
    public abstract class GameMasterBase : MonoBehaviour
    {        
        private static GameMasterBase _baseInstance;
        public static GameMasterBase BaseInstance => _baseInstance;
        
        [SerializeField] protected BaseMainConfig _baseMainConfig;
        public static BaseMainConfig BaseMainConfig => _baseInstance._baseMainConfig;
        
#if USE_FIREBASE
        private FirebaseManager _firebaseManager;
        public static FirebaseManager FirebaseManager { get => BaseInstance._firebaseManager; set => BaseInstance._firebaseManager = value; }
#endif
        private JsonSerializer _jsonSerializer = new JsonSerializer();
        private TimeManager _timeManager = new TimeManager();
        private NetworkManager _networkManager;
        private AudioManager _audioManager;

        public static JsonSerializer JsonSerializer => GameMasterBase._baseInstance._jsonSerializer;
        public static TimeManager TimeManager => _baseInstance._timeManager;
        public static NetworkManager NetworkManager => _baseInstance._networkManager ?? (_baseInstance._networkManager = new NetworkManager());
        public static AudioManager AudioManager { get => _baseInstance._audioManager; set => _baseInstance._audioManager = value;}

        [NonSerialized] public bool IsGameLoaded;
        
        protected virtual void Awake()
        {
            _baseInstance = this;
        }        
        
        protected virtual void Start()
        {

        }
        
        public IEnumerator SetupScreenSleepTimeout()
        {
            Screen.sleepTimeout = (int)0f;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            yield return null;
        }
        
        public abstract string GetUserId();
    }
}
