using UnityEngine;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    public struct PlayerHealthChangedEvent
    {
        public int NewHealth;
    }

    public class EventBusSample : MonoBehaviour
    {
        private int _currentHealth = 100;

        private void OnEnable()
        {
            EventBus<PlayerHealthChangedEvent>.Register(OnHealthChanged);
        }

        private void OnDisable()
        {
            EventBus<PlayerHealthChangedEvent>.Deregister(OnHealthChanged);
        }

        // Public method to be called by the Bootstrap UI Button
        public void RaiseEvent()
        {
            _currentHealth -= 10;
            if (_currentHealth < 0) _currentHealth = 100;

            Debug.Log($"Raising health changed event... (New Health: {_currentHealth})");
            EventBus<PlayerHealthChangedEvent>.Raise(new PlayerHealthChangedEvent { NewHealth = _currentHealth });
        }

        private void OnHealthChanged(PlayerHealthChangedEvent evt)
        {
            Debug.Log($"[Event Received] Health changed to: {evt.NewHealth}");
        }
    }
}
