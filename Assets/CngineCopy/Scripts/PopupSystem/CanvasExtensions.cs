using UnityEngine;

namespace Cngine
{
    public static class CanvasExtensions
    {
        public static void SetDepth(this Canvas cnv, int depth)
        {
            if (cnv.gameObject.activeInHierarchy && cnv.enabled)
            {
                cnv.overrideSorting = true;
                cnv.sortingOrder = depth;
                return;
            }

            Log.Error("Can't set depth the popup is disabled");
        }
    }
}