using UnityEngine;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    public class ObjectPoolSample : MonoBehaviour
    {
        [SerializeField] private GameObjectPool _pool;

        public GameObjectPool Pool
        {
            get => _pool;
            set => _pool = value;
        }

        // Public method to be called by the Bootstrap UI Button
        public void SpawnObject()
        {
            if (_pool != null)
            {
                var instance = _pool.Get();
                instance.transform.position = Random.insideUnitSphere * 5f;

                // Return to pool after 2 seconds
                StartCoroutine(ReturnToPoolRoutine(instance));
            }
            else
            {
                Debug.LogWarning("GameObjectPool is not assigned in ObjectPoolSample!");
            }
        }

        private System.Collections.IEnumerator ReturnToPoolRoutine(GameObject instance)
        {
            yield return new WaitForSeconds(2f);
            if (_pool != null)
            {
                _pool.Release(instance);
            }
        }
    }
}
