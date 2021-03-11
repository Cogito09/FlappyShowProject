using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cngine
{
    public abstract class BaseSceneBehaviour : MonoBehaviour
    {
        public abstract int GetType();
        [SerializeField] private bool _isBackButtonBlocked;
        
        public bool IsBackButtonBlocked => _isBackButtonBlocked;

        private void Awake()
        {
            RegisterSceneOnSceneManager();
        }

        public abstract void RegisterSceneOnSceneManager();
        public abstract void SetupLoadParameters(object parameter);
        public abstract List<IEnumerator> GetLoadActions();
        public abstract List<IEnumerator> GetUnLoadActions();
        
        protected abstract IEnumerator Setup();

        public void Show(Action onEnabled)
        {
            gameObject.Activate();
            onEnabled?.Invoke();
            
            StartCoroutine(Setup());
        }

        public void Hide(Action onFinish)
        {
            gameObject.Deactivate();
            onFinish?.Invoke();
        }
    }
}