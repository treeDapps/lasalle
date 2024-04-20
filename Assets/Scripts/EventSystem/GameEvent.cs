using System.Collections.Generic;

using UnityEngine;

namespace AuraXR.EventSystem
{
    /// <summary>
    /// Abstract class that receives generic arguments
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public abstract class GameEvent<T> : ScriptableObject
    {
#region Fields and properties

        protected List<IEventListener<T>> listeners = new List<IEventListener<T>>();

#endregion

#region Public methods

        /// <summary>
        /// Raise event with generic parameter
        /// </summary>
        public virtual void Raise(T value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(value);
            }
        }

        /// <summary>
        /// Adds a new generic listener to our listeners list
        /// </summary>
        /// <param name="listener"></param>
        public void RegisterListener(IEventListener<T> listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        /// <summary>
        /// Removes a generic listener from our listeners list
        /// </summary>
        /// <param name="listener"></param>
        public void UnregisterListener(IEventListener<T> listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }

#endregion

#region Unity methods

        protected void OnDisable()
        {
            listeners.Clear();
        }

#endregion

#region Inspector utilities

#if UNITY_EDITOR
        [SerializeField] T debugValue;

        /// <summary>
        /// For testing purposes used through the inspector.
        /// </summary>

        public void RaiseWithDebugValue()
        {
            Raise(debugValue);
        }

#endif

#endregion
    }

    /// <summary>
    /// Base class for scriptable object events.
    /// </summary>
    public abstract class GameEvent : ScriptableObject
    {
#region Fields and properties

        protected List<IEventListener> listeners = new List<IEventListener>();

#endregion

#region Public methods

        /// <summary>
        /// Raise all the listeners in our list
        /// </summary>
        public virtual void Raise()
        {
            foreach (var item in listeners)
            {
                item.OnEventRaised();
            }
        }

        /// <summary>
        /// Adds a new generic listener to our listeners list
        /// </summary>
        /// <param name="listener"></param>
        public void RegisterListener(IEventListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        /// <summary>
        /// Removes a generic listener from our listeners list
        /// </summary>
        /// <param name="listener"></param>
        public void UnregisterListener(IEventListener listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }

#endregion

#region Unity methods

        void OnDisable()
        {
            listeners.Clear();
        }

#endregion
    }
}