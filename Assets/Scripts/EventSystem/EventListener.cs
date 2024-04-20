using UnityEngine;
using UnityEngine.Events;

namespace AuraXR.EventSystem
{
    /// <summary>
    /// Abstract listener with generic parameters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventListener<T> : MonoBehaviour, IEventListener<T>
    {
        protected abstract void OnEnable();
        protected abstract void OnDisable();
        public abstract void OnEventRaised(T data);
    }

    public class EventListener : MonoBehaviour, IEventListener
    {
        public GameEvent gameEvent;
        public UnityEvent response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            response.Invoke();
        }
    }

    public interface IEventListener
    {
        void OnEventRaised();
    }

    public interface IEventListener<T>
    {
        void OnEventRaised(T arg0);
    }
}