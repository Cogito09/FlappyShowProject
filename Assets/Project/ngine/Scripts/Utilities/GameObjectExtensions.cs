using System.Collections.Generic;
using UnityEngine;

namespace Cngine
{
    public static class GameObjectExtensions
    {
        public static void Deactivate(this GameObject go)
        {
            if (go != null && go.activeSelf)
            {
                go.SetActive(false);
            }
        }

        public static void Activate(this GameObject go)
        {
            if (go != null && go.activeSelf == false)
            {
                go.SetActive(true);
            }
        }

        public static void ChangeActive(this GameObject go, bool value)
        {
            if (value)
            {
                go?.Activate();
            }
            else
            {
                go?.Deactivate();
            }
        }
        
        public static void GetAllComponentsUnder<T>(this GameObject go, List<T> toFill) where T : MonoBehaviour
        {
            if (go == null)
            {
                return;
            }

            var t = go.transform;
            if (t == null)
            {
                return;
            }

            foreach (Transform child in t)
            {
                var comp = child.GetComponents<T>();
                if (comp != null)
                {
                    toFill.AddRange(comp);
                }

                GetAllComponentsUnder(child.gameObject, toFill);
            }
        }
    }
}