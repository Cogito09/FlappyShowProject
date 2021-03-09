using System.Collections;
using System.Collections.Generic;
using Cngine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flappy
{
    public class FlappySceneLoaderBehaviour : BaseSceneBehaviour
    {
        public int FlappySceneIndex;
        private GameObject PoolMarker;
        
        public override int GetType()
        {
            return FlappySceneIndex;
        }

        public override void RegisterSceneOnSceneManager()
        {
            GameMaster.ScenesManager.RegisterScene(this);
        }

        public override void SetupLoadParameters(object parameter)
        {   
            //Config or additional data may be passed here.
        }

        public override List<IEnumerator> GetLoadActions()
        {
            return new List<IEnumerator>()
            {
                LoadScene(),
                InitalizePool(),
            };
        }
        
        public override List<IEnumerator> GetUnLoadActions()
        {
            return new List<IEnumerator>()
            {
                UnloadScene(),
                DisposePool(),
            };
        }
        
        private IEnumerator InitalizePool()
        {
            PoolMarker =  new GameObject("--------Pool------------------");
            GameMaster.PoolManager.InitNewPoolObjects(PoolMarker.transform, MainConfig.FlappyGameplayPoolConfig);
            yield return null;
        }

        private IEnumerator DisposePool()
        {
            Destroy(PoolMarker);
            GameMaster.PoolManager.DisposePoolObjects(MainConfig.FlappyGameplayPoolConfig);
            yield return null;
        }
        
        private IEnumerator LoadScene()
        {
            var asyncOperation = SceneManager.LoadSceneAsync(FlappySceneIndex,LoadSceneMode.Additive);
            yield return new WaitWhile(() => asyncOperation.isDone == false);
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(FlappySceneIndex));
            Log.Info($"FlappyScene of index {FlappySceneIndex} Loaded");
        }
        
        private IEnumerator UnloadScene()
        {
            var asyncOperation = SceneManager.UnloadSceneAsync(FlappySceneIndex);
            yield return new WaitWhile(() => asyncOperation.isDone == false);
            Log.Info($"FlappyScene of index {FlappySceneIndex} Unloaded");
        }
        
        protected override IEnumerator Setup()
        {
            EventManager.OnFlappyRoundReseted?.Invoke();
            GameMaster.PopupManager.Show(FlappyPopupType.FirstLoadPopup);
            yield return null;
        }
    }
}