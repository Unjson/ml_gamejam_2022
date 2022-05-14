using medialesson.Library.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace medialesson.Library.Events
{
    [CreateAssetMenu(fileName = "New EventRelay", menuName = "ScriptableObjects/Events/Event Relay")]
    public class EventRelay : ScriptableObject
    {
        protected readonly List<UnityAction> listeners = new List<UnityAction>();

        public virtual void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                if (listeners[i] == null)
                {
                    listeners.RemoveAt(i);
                    continue;
                }

                var curentAction = listeners[i];
                UnityMainThreadDispatcher.Instance.Enqueue(() => curentAction.Invoke());
            }
        }

        public virtual void Add(UnityAction listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public virtual void Remove(UnityAction listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }
    }

    public abstract class EventRelay<T> : ScriptableObject where T : notnull
    {
        protected readonly List<UnityAction<T>> listeners = new List<UnityAction<T>>();

        public virtual void Raise(T value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                if (listeners[i] == null)
                {
                    listeners.RemoveAt(i);
                    continue;
                }

                var curentAction = listeners[i];
                UnityMainThreadDispatcher.Instance.Enqueue(() => curentAction.Invoke(value));
            }
        }

        public virtual void Add(UnityAction<T> listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public virtual void Remove(UnityAction<T> listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }
    }
}


