using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Flappy.Editor
{
    public class ObstacleAttributeDrawer : OdinAttributeDrawer<ObstacleAttribute,int>
    {
        private Dictionary<int, string> _toDisplay = new Dictionary<int, string>();
        private List<int> _ids;

        public override bool CanDrawTypeFilter(Type type)
        {
            return type == typeof(int);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var prefabs = MainConfig.FlappyObstaclesConfig.Obstacles;
            if (prefabs == null)
            {
                return;
            }

            _toDisplay.Clear();
            _toDisplay.Add(0, "Undefined");
            _ids = new List<int>() { 0 };

            for (int i = 0; i < prefabs.Count; i++)
            {
                _toDisplay.Add(prefabs[i].Id, $"{prefabs[i].Id}.{prefabs[i].DevName.ToString()}");
                _ids.Add(prefabs[i].Id);
            }

            var values = _toDisplay.Values.ToArray();
            var indexOfCurrentItem = _ids.IndexOf(ValueEntry.SmartValue);
            if (indexOfCurrentItem <= 0)
            {
                indexOfCurrentItem = 0;
            }

            var index = EditorGUILayout.Popup(label, indexOfCurrentItem, values);
            ValueEntry.SmartValue = _ids[index];
        }
    }
}