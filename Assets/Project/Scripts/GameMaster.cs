using Cngine;
using Cngine.PopupSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Flappy
{
    public class GameMaster : GameMasterBase
    {
        private static GameMaster _instance;
        
        [SerializeField] private ScenesManager _sceneManager;
        private MainConfig _mainConfig;
        private PopupManager<FlappyPopupType> _popupManager = new PopupManager<FlappyPopupType>(100,50);
        private PoolManager<FlappyPrefabTemplate, FlappyPrefabsConfig> _poolManager;
        private FacebookManager _facebookManager = new FacebookManager();
        private FlappyScoreManager _flappyScoreManager;

        public static MainConfig MainConfig => _instance._mainConfig ??= (MainConfig) GameMasterBase.BaseMainConfig;
        public static PoolManager<FlappyPrefabTemplate, FlappyPrefabsConfig> PoolManager => _instance._poolManager ??=  new PoolManager<FlappyPrefabTemplate, FlappyPrefabsConfig>(MainConfig.PrefabsConfig);
        public static FlappyScoreManager FlappyScoreManager => _instance._flappyScoreManager ??= new FlappyScoreManager();
        public static ScenesManager ScenesManager => _instance._sceneManager;
        public static PopupManager<FlappyPopupType> PopupManager => _instance._popupManager;
        public static FacebookManager FacebookManager => _instance._facebookManager;
        
        private Save _save = new Save();
        public static Save Save => _instance._save;
        

        public override string GetUserId()
        {
            throw new System.NotImplementedException();
        }

        protected override void Awake()
        {        
            _instance = this;
            base.Awake();
            FacebookManager.TryInitialize();
        }
    }
}


