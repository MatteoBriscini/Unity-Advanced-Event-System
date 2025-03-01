using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Script.Events.EventsLayout
{
    public class BasicEventChannel : ScriptableObject
    {
        private readonly List<Action> _callbacks = new List<Action>();
        private Action<List<Action>> _handleCallback;

        /// <summary>
        /// Assigns a handler function to process callbacks when the event is raised.
        /// </summary>
        /// <param name="handleCallback">The callback handler function.</param>
        public void AddCallBackHandler(Action<List<Action>> handleCallback)
        {
            _handleCallback = handleCallback;
        }

        /// <summary>
        /// Raises the event and invokes all subscribed callbacks.
        /// </summary>
        public void RaiseEvent()
        {
            List<Action> actionsCopy = new List<Action>(_callbacks);
            if (_handleCallback != null) _handleCallback(actionsCopy);
            else
            {
                Debug.LogError("empty callback handler");
            }
        }

        /// <summary>
        /// Subscribes a callback to the event channel.
        /// </summary>
        /// <param name="callback">The callback to subscribe.</param>
        public virtual void Subscribe([NotNull] Action callback)
        {
            _callbacks.Add(callback);
        }

        /// <summary>
        /// Unsubscribes a callback from the event channel.
        /// </summary>
        /// <param name="callback">The callback to unsubscribe.</param>
        public void Unsubscribe([NotNull] Action callback)
        {
            _callbacks.Remove(callback);
        }
    }
}