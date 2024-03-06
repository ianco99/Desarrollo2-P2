using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kuznickiEventChannel
{
    /// <summary>
    /// Class that handles events communication between two gameObjects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventChannelSO<T> : ScriptableObject
    {
        private Action<T> myAction;

        public void Subscribe(in Action<T> handler)
        {
            myAction += handler;
        }

        public void Unsubscribe(in Action<T> handler)
        {
            myAction -= handler;
        }

        public void RaiseEvent(T data)
        {
            if (myAction?.GetInvocationList().Length > 0)
                myAction?.Invoke(data);
        }
    }
}

