using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cngine
{
    public class SceneSwitchBehaviour : LoadingSystem
    {
        // [InfoBox("Implement Animator with 2 animations ,name trigger tags as HideTrigger,ShowTrigger")]
        // [InfoBox("Need to be (coniside UIRoot tains canvas)")]
        // [SerializeField] private Animator _animator;
        // [SerializeField] private static string AnimationTriggerHideString = "HideTrigger";
        // [SerializeField] private static string AnimationTriggerShowString = "ShowTrigger";
        // private int HideTrigger = Animator.StringToHash(AnimationTriggerHideString);
        // private int ShowTrigger = Animator.StringToHash(AnimationTriggerShowString);

        public PotaTween _showTween;
        public PotaTween _hideTween;
        private Action _onShowTweenFinished;
        private Action _onHiddenTweenFinished;

        
        private Action _onFinished;
        private readonly List<IEnumerator> _emptyActions = new List<IEnumerator>();
        private readonly List<IEnumerator> _toDoActions = new List<IEnumerator>();
        
        protected override List<IEnumerator> GetActionsToPreform()
        {
            return _toDoActions;
        }
        
        public void Load(List<IEnumerator> toDo, Action onLoadingFinished)
        {
            if (_isLoading)
            {
                Log.Exception("Cannot load two things at the same time!");
                return;
            }

            gameObject.Activate();
            Show(() =>
            {
                _toDoActions.Clear();
                _toDoActions.AddRange(toDo);
                _onFinished = onLoadingFinished;
                
                LoadManually();
            });
        }
        
        protected override void OnProgressChanged()
        {
        }
        protected override void OnLoadingStarted()
        {
        }
        protected override void OnLoadingFinished()
        {
            _onFinished?.Invoke();
            Hide(gameObject.Deactivate);
        }
        
        public void Show(Action onFinish)
        {
            _onShowTweenFinished = onFinish;
            _showTween.Play();
        }

        public void Hide(Action onFinish)
        {
            _onHiddenTweenFinished = onFinish;
            _hideTween.Play();
        }

        public void OnHideFinishedActionEvent()
        {
            _onHiddenTweenFinished?.Invoke();
        }

        public void OnShowFinishedActionEvent()
        {
            _onShowTweenFinished?.Invoke();
        }
        
    }
}