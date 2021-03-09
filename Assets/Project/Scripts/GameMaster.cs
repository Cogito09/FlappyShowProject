using Cngine;
using Cngine.PopupSystem;
using Project.Scripts.Flappy;
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
        private FlappyScoreManager _flappyScoreManager;
        
        public static MainConfig MainConfig => _instance._mainConfig ??= (MainConfig) GameMasterBase.BaseMainConfig;
        public static PoolManager<FlappyPrefabTemplate, FlappyPrefabsConfig> PoolManager => _instance._poolManager;
        public static FlappyScoreManager FlappyScoreManager => _instance._flappyScoreManager;
        public static ScenesManager ScenesManager => _instance._sceneManager;
        public static PopupManager<FlappyPopupType> PopupManager => _instance._popupManager;
        
        public override string GetUserId()
        {
            throw new System.NotImplementedException();
        }

        protected override void Awake()
        {        
            _instance = this;
            base.Awake();
        }
    }
}


