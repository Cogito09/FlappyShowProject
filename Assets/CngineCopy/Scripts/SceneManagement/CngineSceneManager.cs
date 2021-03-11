using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cngine
{
    public abstract class CngineSceneManager<T> : MonoBehaviour where T : struct,IConvertible
    {
        private Action HideAllPopups;
        private SceneSwitchBehaviour _switchBehaviour;
        private BaseSceneBehaviour _currentBaseScene;
        
        private readonly Dictionary<int, BaseSceneBehaviour> _sceneBehaviours = new Dictionary<int, BaseSceneBehaviour>();
        protected int Currentscene => _currentBaseScene?.GetType() ?? 0;
        
        public void Switch(T sceneType,object parameter = null, Action onFinish = null) 
        {
            int scene = sceneType.ToInt32(null);
            if (_currentBaseScene != null && _currentBaseScene.GetType() == scene && _currentBaseScene.GetLoadActions() == null)
            {
                onFinish?.Invoke();
                return;
            }

            _sceneBehaviours.TryGetValue(scene, out var sceneGo);
            if (sceneGo == null)
            {
                Log.Exception($"Didn't find the scene of type {scene}. Check if scene is registered");
                onFinish?.Invoke();
                return;
            }
            
            if (_switchBehaviour == null)
            {
                Log.Exception($"Didn't find the scene switcher. Check if SceneSiwthcer is registered is registered");
                return;
            }

            var switchActions = new List<IEnumerator>();
            switchActions.AddRange(_currentBaseScene?.GetUnLoadActions() ?? new List<IEnumerator>());
            sceneGo.SetupLoadParameters(parameter);
            switchActions.AddRange(sceneGo.GetLoadActions());

            HideAllPopups?.Invoke();
            _switchBehaviour.Load(switchActions,() =>
            {
                _currentBaseScene = sceneGo;
                _currentBaseScene.Show(onFinish);
            });
        }

        public void Initialize<T>( Action HideAllPopupsAction)
        {
            HideAllPopups = HideAllPopupsAction;
        }
        
        public void RegisterSwitcher(SceneSwitchBehaviour switchBehaviour)
        {
            if (_switchBehaviour != null && switchBehaviour != _switchBehaviour)
            {
                Log.Exception("There are spawned two different switchers! Destroying additional one!!!");
                Destroy(switchBehaviour.gameObject);
                return;
            }

            _switchBehaviour = switchBehaviour;
        }

        public void RegisterScene(BaseSceneBehaviour behaviour)
        {
            if (_sceneBehaviours.ContainsKey(behaviour.GetType()))
            {
                Log.Exception($"Already registered scene of type: {behaviour.GetType()}");
                return;
            }

            _sceneBehaviours[behaviour.GetType()] = behaviour;
        }
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}