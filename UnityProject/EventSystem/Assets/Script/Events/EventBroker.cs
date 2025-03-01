using System;
using System.Collections.Generic;
using Script.Events.EventsLayout;
using JetBrains.Annotations;
using UnityEngine;

namespace Script.Events
{
    public class ChannelException : Exception
    {
        public ChannelException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Manages event channels and subscriptions in the event system.
    /// </summary>
    public static class EventBroker
    {
        private static Dictionary<string, BasicEventChannel> _events = new Dictionary<string, BasicEventChannel>();
        private static CallbacksHandler _callbacksHandler;

        /// <summary>
        /// Static constructor to initialize the EventBroker.
        /// Attempts to find the "EventSystem" object and retrieve its CallbacksHandler component.
        /// </summary>
        static EventBroker()
        {
            try
            {
                var eventSystemObject = UnityEngine.GameObject.Find("EventSystem");
                _callbacksHandler = eventSystemObject.GetComponent<CallbacksHandler>();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Failed to initialize EventBroker: " + e.Message);
            }
        }

        /// <summary>
        /// Resets the event system by clearing all registered event channels.
        /// </summary>
        public static void ResetEventSystem()
        {
            _events = new Dictionary<string, BasicEventChannel>();
        }

        /// <summary>
        /// Reassigns the callback handler and updates all existing event channels.
        /// </summary>
        public static void SetCallBackHandler()
        {
            _callbacksHandler = CallbacksHandler.Instance;
            if (_callbacksHandler == null)
            {
                Debug.LogError("CallbacksHandler instance is null. Ensure it exists in the scene.");
            }

            foreach (var e in _events.Values)
            {
                e.AddCallBackHandler(_callbacksHandler.HandleCallback);
            }
        }

        /// <summary>
        /// Adds a new event channel to the system.
        /// </summary>
        /// <param name="eventName">The name of the event channel.</param>
        /// <param name="channel">The event channel to add.</param>
        /// <exception cref="ChannelException">Thrown if the event channel already exists.</exception>
        public static void AddEventChannel(string eventName, BasicEventChannel channel)
        {
            var _channel = channel;
            if (!_events.TryAdd(eventName, channel))
                throw new ChannelException("Channel already exists");
            if (_callbacksHandler != null) _channel.AddCallBackHandler(_callbacksHandler.HandleCallback);
        }

        /// <summary>
        /// Tries to add an event channel; if it already exists, returns the existing one.
        /// </summary>
        /// <param name="eventName">The name of the event channel.</param>
        /// <param name="channel">The event channel to add.</param>
        /// <returns>The newly added or existing event channel.</returns>        
        public static BasicEventChannel TryToAddEventChannel(string eventName, BasicEventChannel channel) {
            BasicEventChannel eventChannel = channel;
            try
            {
                AddEventChannel(eventName, channel);
            }
            catch (ChannelException)
            {
                eventChannel = GetEventChannel(eventName);
            }

            return eventChannel;
        }

        /// <summary>
        /// Subscribes a callback to an event channel.
        /// </summary>
        /// <param name="eventName">The name of the event channel.</param>
        /// <param name="callback">The callback function to subscribe.</param>
        /// <returns>The event channel the callback was subscribed to.</returns>
        /// <exception cref="System.Exception">Thrown if the event channel does not exist.</exception>
        public static BasicEventChannel SubsToEventChannel(string eventName, [NotNull] Action callback) {
            if (!_events.TryGetValue(eventName, out BasicEventChannel channel))
                throw new System.Exception("Channel does not exist");
            channel.Subscribe(callback);
            return channel;
        }

        /// <summary>
        /// Unsubscribes a callback from an event channel.
        /// </summary>
        /// <param name="eventName">The name of the event channel.</param>
        /// <param name="callback">The callback function to unsubscribe.</param>
        /// <exception cref="System.Exception">Thrown if the event channel does not exist.</exception>
        public static void UnsubFromEventChannel(string eventName, [NotNull] Action callback)
        {
            if (!_events.TryGetValue(eventName, out BasicEventChannel channel))
                throw new System.Exception("Channel does not exist");
            channel.Unsubscribe(callback);
        }

        /// <summary>
        /// Retrieves an event channel by name.
        /// </summary>
        /// <param name="eventName">The name of the event channel.</param>
        /// <returns>The requested event channel.</returns>
        /// <exception cref="System.Exception">Thrown if the event channel does not exist.</exception>
        public static BasicEventChannel GetEventChannel(string eventName) {
            if (!_events.TryGetValue(eventName, out BasicEventChannel channel))
                throw new System.Exception("Channel " + eventName + " does not exist");
            return channel;
        }
    }
}