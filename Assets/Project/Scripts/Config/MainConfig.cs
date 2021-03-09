using Cngine;
using UnityEngine;

namespace Flappy
{
    [CreateAssetMenu(fileName = "MainConfig", menuName = "Configs/MainConfig")]
    public class MainConfig : BaseMainConfig
    {
        public static FlappyPrefabsConfig PrefabsConfig => Instance._prefabsConfig;
        public static PopupConfig PopupConfig => Instance.popupConfig;
        public static LoadingConfig LoadingConfig => Instance._loaderConfig;
        public static PoolConfig FlappyGameplayPoolConfig => Instance._flappyPoolConfig;
        public static FlappyGameplayConfig FlappyGameplayConfig => Instance._flappyGameplayConfig;
        [SerializeField] private PopupConfig popupConfig;
        [SerializeField] private LoadingConfig _loaderConfig;
        [SerializeField] private FlappyPrefabsConfig _prefabsConfig;
        [SerializeField] private PoolConfig _flappyPoolConfig;
        [SerializeField] private FlappyGameplayConfig _flappyGameplayConfig;
        public override LoadingConfig GetLoadingConfig()
        {
            return LoadingConfig;
        }

        public override BasePopupConfig GetPopupConfig()
        {
            return PopupConfig;
        }
        
        private static MainConfig _mainConfig;
        public static MainConfig Instance =>
#if !UNITY_EDITOR
             _mainConfig ?? (_mainConfig = GameMaster.MainConfig);
#endif
#if UNITY_EDITOR
        _mainConfig ?? Application.isPlaying == false ? (_mainConfig = (MainConfig) GetConfigAsset()) : GameMaster.MainConfig;



        private static MainConfig GetConfigAsset()
        {
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<MainConfig>("Assets/Project/Configs/MainConfig.asset");
            return asset;
        }
#endif
    }
}