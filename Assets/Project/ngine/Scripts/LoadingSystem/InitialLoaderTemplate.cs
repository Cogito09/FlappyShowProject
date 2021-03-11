using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cngine
{
    public abstract class InitialLoaderTemplate : LoadingSystem
    {
        [SerializeField] private Transform _uiRoot;
        [SerializeField] private Transform LoadersMarker;
        [SerializeField] private Transform PrefabsMarker;
        [SerializeField] private SceneSwitchBehaviour _sceneSwitchBehaviour;
        
        private const string LOADING_STARTED_EVENT = "loading_started";
        private const string LOADING_FINISHED_EVENT = "loading_finished";
        private const string LOADING_CHECK_CONSENT = "loading_checking_gdpr";
        private const string LOADING_CHECK_CONSENT_FINISHED = "loading_checking_gdpr_finished";
        private const string LOADING_PROGRESS_EVENT = "loading_progress";

        protected abstract IEnumerator InitializeGameSceneManager();
        protected abstract void AddLoadActions(ref List<IEnumerator> list);
        protected abstract IEnumerator LoadStartingScene();
        private static Action<Popup,int> RegisterPopupAction;


        private void Start()
        {
            LoadManually();
        }
        
        protected void SetRegisterPopupAction(Action<Popup, int> registerPopupAction)
        {            
            RegisterPopupAction = registerPopupAction;
        }
        
        protected override void OnProgressChanged()
        {
            SendAnalytycsEvent(LOADING_PROGRESS_EVENT);
        }

        protected override List<IEnumerator> GetActionsToPreform()
        {
            var list = new List<IEnumerator>();

            AddInitPrefabsToLoad(ref list);
            AddInitLoadersToLoad(ref list);
            AddInitUIPrefabsToLoad(ref list);
            AddInitPopupsToLoad(ref list);
            
            list.Add(RegisterSceneSwitcherToGameSceneManager(_sceneSwitchBehaviour));
            list.Add(GameMasterBase.BaseInstance.SetupScreenSleepTimeout());
            list.Add(InitializeGameSceneManager());
            list.Add(SendAnalytycsEvent(LOADING_CHECK_CONSENT));
            list.Add(SendAnalytycsEvent(LOADING_CHECK_CONSENT_FINISHED));
            //Here are actions to be implemented
            AddLoadActions(ref list);
            //Now its time for Finishing Actions
            list.Add(LoadStartingScene());
            list.Add(OnFinishLoad());
            return list;
        }

        protected abstract IEnumerator RegisterSceneSwitcherToGameSceneManager(SceneSwitchBehaviour sceneSwitcher);

        protected override void OnLoadingFinished()
        {
            SendAnalytycsEvent(LOADING_FINISHED_EVENT);
            gameObject.Deactivate();
        }

        protected override void OnLoadingStarted()
        {
            SendAnalytycsEvent(LOADING_STARTED_EVENT);
            gameObject.Activate();
        }



        private IEnumerator OnFinishLoad()
        {
            GameMasterBase.BaseInstance.IsGameLoaded = true;
            yield return null;
        }

        private IEnumerator SendAnalytycsEvent(string eventName)
        {
            // if (args == null || args.Length <= 0)
            // {
            //     GameMaster.Analytics.SendEvent(eventName);
            // }
            // else
            // {
            //     var arguments = new List<Parameter>(args)
            //     {
            //         new Parameter("progress", Progress)
            //     };
            //
            //     GameMaster.Analytics.SendEvent(eventName, arguments.ToArray());
            // }
            //
            yield break;
        }
        
        public struct LoadPopupAction
        {
            public Transform Parent;
            public MonoBehaviour Data;
            public int Type;
            public bool IsStaticPanel;

            public IEnumerator Preform()
            {
                if (Parent == null)
                {
                    Instantiate(Data);
                }

                var popup = Instantiate(Data, Parent);
                if (IsStaticPanel)
                {
                    yield break;
                }

                var script = popup.GetComponent<Popup>();
                if (script == null)
                {
                    yield break;
                }

                if (script.gameObject.activeSelf == true)
                {
                    Log.Warning(
                        $"{script.gameObject.name} popup prefab is active on loaded. Make sure to deactivate it for better preformance.");
                    script.gameObject.Deactivate();
                }
                
                if (RegisterPopupAction == null)
                {
                    Log.Error("Pass RegisterAction to Setup on Awake of InitialLoader.");
                }
                
                RegisterPopupAction?.Invoke(script,Type);
            }
        }

        public struct LoadPrefabAction
        {
            public int? SiblingIndex;
            public Transform Parent;
            public GameObject Data;

            public IEnumerator Preform()
            {
                var obj = Parent == null ? Instantiate(Data) : Instantiate(Data, Parent);
                var spawnCallbackReceiver = obj.GetComponent<ISpawnCallbackReceiver>();
                spawnCallbackReceiver?.OnInstantiated();
                if (SiblingIndex != null)
                {
                    obj.transform.SetSiblingIndex((int) SiblingIndex);
                }

                yield return null;
            }
        }

        private void AddInitPopupsToLoad(ref List<IEnumerator> loadActions)
        {
            var allPopupsToLoad = GameMasterBase.BaseMainConfig.GetPopupConfig().GetPopupsToLoad();
            for (int i = 0; i < allPopupsToLoad.Count; i++)
            {
                if (allPopupsToLoad[i].GetPrefab() == null)
                {
                    Log.Error($"Popup prefab of PopupType : {allPopupsToLoad[i].GetPopupType()} is null");
                    continue;
                }

                var loadPopupAction = new LoadPopupAction()
                {
                    Parent = _uiRoot.transform,
                    Data = allPopupsToLoad[i].GetPrefab(),
                    Type = allPopupsToLoad[i].GetPopupType(),
                    IsStaticPanel = allPopupsToLoad[i].GetIfIsStaticField()
                };

                loadActions.Add(loadPopupAction.Preform());
            }
        }

        private void AddInitPrefabsToLoad(ref List<IEnumerator> list)
        {
            var allPrefabsToLoad = GameMasterBase.BaseMainConfig.GetLoadingConfig().PrefabsToBeSpawnedAtInit.ToList();
            var siblingIndex = PrefabsMarker.GetSiblingIndex();
            for (int i = 0; i < allPrefabsToLoad.Count; i++)
            {
                if (allPrefabsToLoad[i] == null)
                {
                    Log.Error($"Prefab of : {allPrefabsToLoad[i]} is null");
                    continue;
                }

                var loadPrefabAction = new LoadPrefabAction()
                {
                    SiblingIndex = siblingIndex,
                    Data = allPrefabsToLoad[i]
                };

                list.Add(loadPrefabAction.Preform());
            }
        }

        private void AddInitUIPrefabsToLoad(ref List<IEnumerator> list)
        {
            var allPrefabsToLoad = GameMasterBase.BaseMainConfig.GetLoadingConfig().UIPrefabsToBeSpawnedAtInit.ToList();
            for (int i = 0; i < allPrefabsToLoad.Count; i++)
            {
                if (allPrefabsToLoad[i] == null)
                {
                    Log.Error($"Prefab of : {allPrefabsToLoad[i]} is null");
                    continue;
                }

                var loadPopupAction = new LoadPrefabAction()
                {
                    SiblingIndex = null,
                    Data = allPrefabsToLoad[i],
                    Parent = _uiRoot
                };

                list.Add(loadPopupAction.Preform());
            }

        }

        private void AddInitLoadersToLoad(ref List<IEnumerator> list)
        {
            var loaderObjects = GameMasterBase.BaseMainConfig.GetLoadingConfig().SceneLoadersToBeSpawnedAtInit.ToList();
            var siblingIndex = LoadersMarker.GetSiblingIndex();
            for (int i = 0; i < loaderObjects.Count; i++)
            {
                if (loaderObjects[i] == null)
                {
                    Log.Error($"Loader prefab {loaderObjects[i]} is null");
                    continue;
                }

                var loadPopupAction = new LoadPrefabAction()
                {
                    SiblingIndex = siblingIndex,
                    Data = loaderObjects[i]
                };

                list.Add(loadPopupAction.Preform());
            }
        }

    }


}