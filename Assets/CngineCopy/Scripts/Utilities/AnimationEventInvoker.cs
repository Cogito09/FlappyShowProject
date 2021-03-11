using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cngine
{
    public class AnimationEventInvoker : MonoBehaviour
    {
        public UnityEvent UnityEvent1;
        public UnityEvent UnityEvent2;
        public UnityEvent UnityEvent3;

        public void ExecuteEvent1()
        {
            UnityEvent1?.Invoke();
        }
        public void ExecuteEvent2()
        {
            UnityEvent2?.Invoke();
        }
        public void ExecuteEvent3()
        {
            UnityEvent3?.Invoke();
        }
    }
}


