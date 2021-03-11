using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cngine.PopupSystem
{
    public class PopupManager<T> where T : struct, IConvertible
    {
        private readonly Dictionary<T, IPopup> _registeredPopups = new Dictionary<T, IPopup>();
        private readonly Dictionary<T,PopupData<T>> _openedPopups = new Dictionary<T,PopupData<T>>();
        private readonly HashSet<int> Depths = new HashSet<int>();
        private readonly int _depthGaps;
        private Coroutine _hideAllCoroutine;
        public readonly int InitialDepth;
        public int CurrentHighestDepth { get; private set; }
        public int TargetDepth => InitialDepth + CurrentHighestDepth;

        public PopupManager(int initialDepth, int depthGaps)
        {
            InitialDepth = initialDepth;
            _depthGaps = depthGaps;
            CurrentHighestDepth = 0;
        } 

        public void Show(T type, object param = null, int forcedDepth = 0)
        {
            var popup = GetRegisteredPopup(type);
            if (popup == null)
            {
                return;
            }

            _openedPopups.TryGetValue(type, out var pop);
            if (pop != null)
            {
                Log.Info("Returning, same popup wanted to be opened multiple times!");
                return;
            }
            
            CurrentHighestDepth += _depthGaps;
            Depths.Add(CurrentHighestDepth);
            var popupData = new PopupData<T>
            {
                Popup = popup,
                Data = param,
                Depth = CurrentHighestDepth,
                PopupType = type
            };

            _openedPopups.Add(type,popupData);
            popup.AnimateShow(() =>
            {
                popup.IsVisible = true;
                popup.Setup(popupData.Data);
            });

            popup.SetDepth(popupData.Depth);
        }
        
        public void Hide(T type, Action onHidden = null)
        {
            if (IsOpened(type) == false)
            {
                onHidden?.Invoke();
                return;
            }

            _openedPopups.TryGetValue(type, out var popupToHide);
            if (popupToHide == null)
            {
                Log.Error($"Popup of type {type} not found.");
                return;
            }

            _openedPopups.Remove(type);
            Depths.Remove(popupToHide.Depth);
            popupToHide.Popup.AnimateHide(() =>
            {
                CurrentHighestDepth = Depths.Count > 0 ? Depths.Max() : InitialDepth;
                onHidden?.Invoke();
            });
        }

        public bool IsAnythingOpened()
        {
            return _openedPopups.Count > 0;
        }

        public bool IsOpened(T type)
        {
            foreach (var pop in _openedPopups)
            {
                if (pop.Value.PopupType.Equals(type))
                {
                    return true;
                }
            }

            return false;
        }

        public void HideAll()
        {
            HideAll(null);
        }
        
        public void HideAll(Action onFinish = null)
        {
            if (IsAnythingOpened() == false)
            {
                onFinish?.Invoke();
                return;
            }


            if (_hideAllCoroutine != null)
            {
                Log.Info("Hide All in progress...");
                return;
            }

            _hideAllCoroutine = GameMasterBase.BaseInstance.StartCoroutine(HideAllInCoroutine(onFinish));
        }

        private IEnumerator HideAllInCoroutine(Action onFinish)
        {
            if (IsAnythingOpened() == false)
            {
                onFinish?.Invoke();
                yield break;
            }
            
            CurrentHighestDepth = 0;
            var index = 0;
            foreach (var pop in _openedPopups)
            {
                pop.Value.Popup.AnimateHide(() =>
                {
                    index--;
                    pop.Value.Popup.IsVisible = false;
                });
            }

            _openedPopups.Clear();
            var time = Time.time;
            var delta = 0f;
            while (index > 0 && delta < 10)
            {
                delta = Time.time - time;
                yield return new WaitForEndOfFrame();
            }

            onFinish?.Invoke();
            _hideAllCoroutine = null;
        }
        
        private IPopup GetRegisteredPopup(T type)
        {
            if (_registeredPopups.TryGetValue(type, out var popup) == false)
            {
                Log.Error("Can't find popup of type: " + type);
                return null;
            }

            return popup;
        }

        public void RegisterPopup(IPopup popup, T type)
        {
            if (_registeredPopups.ContainsKey(type))
            {
                Log.Error("Given popup already exists: " + type);
                return;
            }

            popup.SetPopupType(type);
            _registeredPopups[type] = popup;
        }
    }
}