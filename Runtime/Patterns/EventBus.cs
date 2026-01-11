using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// A simple, thread-safe, static event bus for decoupling systems.
    /// Allows publishing and subscribing to strongly-typed events.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();
        private static readonly object _lock = new object();

        /// <summary>
        /// Subscribes a listener to an event of type T.
        /// </summary>
        /// <typeparam name="T">The event type.</typeparam>
        /// <param name="listener">The action to invoke when the event is raised.</param>
        public static void Subscribe<T>(Action<T> listener)
        {
            if (listener == null) return;

            lock (_lock)
            {
                var type = typeof(T);
                if (!_events.ContainsKey(type))
                {
                    _events[type] = null;
                }

                // Combine the new listener with existing delegates
                _events[type] = Delegate.Combine(_events[type], listener);
            }
        }

        /// <summary>
        /// Unsubscribes a listener from an event of type T.
        /// </summary>
        /// <typeparam name="T">The event type.</typeparam>
        /// <param name="listener">The action to remove.</param>
        public static void Unsubscribe<T>(Action<T> listener)
        {
            if (listener == null) return;

            lock (_lock)
            {
                var type = typeof(T);
                if (_events.ContainsKey(type))
                {
                    var currentDelegate = _events[type];
                    var newDelegate = Delegate.Remove(currentDelegate, listener);

                    if (newDelegate == null)
                    {
                        _events.Remove(type);
                    }
                    else
                    {
                        _events[type] = newDelegate;
                    }
                }
            }
        }

        /// <summary>
        /// Raises an event of type T, invoking all subscribed listeners.
        /// Exceptions in listeners are caught and logged, ensuring other listeners are still called.
        /// </summary>
        /// <typeparam name="T">The event type.</typeparam>
        /// <param name="eventArgs">The event arguments.</param>
        public static void Raise<T>(T eventArgs)
        {
            Delegate d;
            lock (_lock)
            {
                if (!_events.TryGetValue(typeof(T), out d))
                {
                    return;
                }
            }

            if (d != null)
            {
                var invocationList = d.GetInvocationList();
                foreach (var handler in invocationList)
                {
                    try
                    {
                        ((Action<T>)handler).Invoke(eventArgs);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(new Exception($"[EventBus] Error handling event {typeof(T).Name}: {ex.Message}", ex));
                    }
                }
            }
        }

        /// <summary>
        /// Clears all event listeners.
        /// Useful when resetting the game or unloading scenes.
        /// </summary>
        public static void Clear()
        {
            lock (_lock)
            {
                _events.Clear();
            }
        }
    }
}
