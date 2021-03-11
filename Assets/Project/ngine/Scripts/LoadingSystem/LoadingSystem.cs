using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cngine
{
    public abstract class LoadingSystem : MonoBehaviour
    {
        private readonly List<IEnumerator> _operations = new List<IEnumerator>();
        protected abstract List<IEnumerator> GetActionsToPreform();
        protected abstract void OnLoadingFinished();
        protected abstract void OnLoadingStarted();
        protected abstract void OnProgressChanged();

        public float Progress => Mathf.Max(0, Mathf.Min((float) _progress / _operationsToDo, 1f));
        public int DoneActions => _progress;
        public int ToDoActions => _operationsToDo;

        protected bool _isLoading;
        private int _operationsToDo;
        private int _progress;
        
        public void LoadManually()
        {
            AssignOperations();
            Load();
        }
        
        private void Load()
        {
            if (_isLoading)
            {
                return;
            }

            OnLoadingStarted();
            StopAllCoroutines();

            _progress = 0;
            _isLoading = true;
            _operationsToDo = _operations.Count;

            StartCoroutine(StartLoading());
        }

        private void AssignOperations()
        {
            _operations.Clear();
            _operations.AddRange(GetActionsToPreform());
        }

        private IEnumerator StartLoading()
        {
            OnProgressChanged();
            for (int i = 0, c = _operations.Count; i < c; ++i)
            {
                var op = _operations[i];
                yield return StartCoroutine(op);
                _progress++;
                OnProgressChanged();
            }

            yield return 0;
            _isLoading = false;
            OnLoadingFinished();
            yield return 0;
        }
    }
}