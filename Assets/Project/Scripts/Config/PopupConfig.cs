using System;
using System.Collections.Generic;
using Cngine;
using UnityEngine;

namespace Flappy
{
    [CreateAssetMenu(fileName = "PopupsConfig", menuName = "Configs/PopupsConfig")]
    public class PopupConfig : BasePopupConfig
    {
        public List<PopupPrefab> PopupPrefabs;
        public override List<IPopupPrefab> GetPopupsToLoad()
        {
            var popups = new List<IPopupPrefab>();
            PopupPrefabs.ForEach(popup => popups.Add(popup));
            return popups;
        }
    }

    [Serializable]
    public struct PopupPrefab : IPopupPrefab
    {
        public PopupType PopupType;
        public MonoBehaviour Prefab;
        public bool IsStaticPanel;
        
        public int GetPopupType()
        {
            return (int) PopupType;
        }

        public MonoBehaviour GetPrefab()
        {
            return Prefab;
        }

        public bool GetIfIsStaticField()
        {
            return IsStaticPanel;
        }
    }
    
    public enum PopupType
    {
        Unknown = 0,
        Menu = 1,
        Score = 2,
    }
}