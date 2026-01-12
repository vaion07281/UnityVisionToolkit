using System;
using UnityEngine;
using UnityEngine.Pool;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// A MonoBehaviour wrapper for Unity's built-in ObjectPool API.
    /// Manages a pool of GameObjects instantiated from a prefab.
    /// </summary>
    public class GameObjectPool : MonoBehaviour
    {
        [Tooltip("The prefab to pool.")]
        [SerializeField] private GameObject _prefab;

        [Tooltip("The initial capacity of the pool.")]
        [SerializeField] private int _defaultCapacity = 10;

        [Tooltip("The maximum size of the pool. If the pool is full, extra items will be destroyed when released.")]
        [SerializeField] private int _maxSize = 100;

        private ObjectPool<GameObject> _pool;

        /// <summary>
        /// Initializes the ObjectPool.
        /// </summary>
        private void Awake()
        {
            if (_prefab == null)
            {
                Debug.LogError($"[GameObjectPool] Prefab is not assigned on {name}!", this);
                enabled = false;
                return;
            }

            _pool = new ObjectPool<GameObject>(
                createFunc: OnCreatePoolItem,
                actionOnGet: OnGetPoolItem,
                actionOnRelease: OnReleasePoolItem,
                actionOnDestroy: OnDestroyPoolItem,
                collectionCheck: true,
                defaultCapacity: _defaultCapacity,
                maxSize: _maxSize
            );
        }

        /// <summary>
        /// Gets an active object from the pool.
        /// </summary>
        /// <returns>An active GameObject.</returns>
        public GameObject Get()
        {
            if (_pool == null)
            {
                Debug.LogError($"[GameObjectPool] Pool is not initialized on {name}.", this);
                return null;
            }
            return _pool.Get();
        }

        /// <summary>
        /// Returns an object to the pool.
        /// </summary>
        /// <param name="obj">The object to release.</param>
        public void Release(GameObject obj)
        {
            if (_pool == null)
            {
                Debug.LogError($"[GameObjectPool] Pool is not initialized on {name}.", this);
                return;
            }
            _pool.Release(obj);
        }

        /// <summary>
        /// Creates a new instance of the prefab.
        /// Parents the new instance to this transform to keep the hierarchy clean.
        /// </summary>
        /// <returns>The new GameObject.</returns>
        private GameObject OnCreatePoolItem()
        {
            GameObject obj = Instantiate(_prefab, transform);
            return obj;
        }

        /// <summary>
        /// Called when an item is retrieved from the pool.
        /// Sets the object to active.
        /// </summary>
        /// <param name="obj">The object being retrieved.</param>
        private void OnGetPoolItem(GameObject obj)
        {
            obj.SetActive(true);
        }

        /// <summary>
        /// Called when an item is returned to the pool.
        /// Sets the object to inactive.
        /// </summary>
        /// <param name="obj">The object being returned.</param>
        private void OnReleasePoolItem(GameObject obj)
        {
            obj.SetActive(false);
        }

        /// <summary>
        /// Called when the pool is full and the item needs to be destroyed.
        /// </summary>
        /// <param name="obj">The object to destroy.</param>
        private void OnDestroyPoolItem(GameObject obj)
        {
            Destroy(obj);
        }
    }
}
