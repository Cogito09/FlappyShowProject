using UnityEngine;
using UnityEngine.UI;

namespace Cngine
{
    public abstract class ScrollSlotBase : MonoBehaviour
    {
        public abstract void ScrollCellIndex(object data);
    }
}