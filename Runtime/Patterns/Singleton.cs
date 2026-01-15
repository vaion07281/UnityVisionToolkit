using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Thread-safe generic Singleton class inheriting from MonoBehaviour.
    /// Automatically creates a GameObject if the instance is null.
    /// Prevents ghost object creation when the application is quitting.
    /// </summary>
    /// <typeparam name="T">The type of the component inheriting from Singleton.</typeparam>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _applicationIsQuitting = false;

        /// <summary>
        /// Gets the instance of the Singleton.
        /// Creates a new GameObject with the component if it does not exist.
        /// Returns null if the application is quitting.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton!" +
                                           " Reopening the scene might fix it.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = typeof(T).Name;

                            Debug.Log($"[Singleton] An instance of {typeof(T)} is needed in the scene, so '{singleton.name}' was created with DontDestroyOnLoad.");
                        }
                    }

                    return _instance;
                }
            }
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// Calls DontDestroyOnLoad by default.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Called when the application quits.
        /// Sets a flag to prevent re-creation of the singleton during shutdown.
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }
    }
}
