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
        private void OnEnable()
        {
            EventBus<PlayerHealthChangedEvent>.Register(OnHealthChanged);
        }

        private void OnDisable()
        {
            EventBus<PlayerHealthChangedEvent>.Deregister(OnHealthChanged);
        }

        private void Start()
        {
            // Simulate health change
            Debug.Log("Raising health changed event...");
            EventBus<PlayerHealthChangedEvent>.Raise(new PlayerHealthChangedEvent { NewHealth = 50 });
        }

        private void OnHealthChanged(PlayerHealthChangedEvent evt)
        {
            Debug.Log($"Health changed to: {evt.NewHealth}");
        }
    }
}