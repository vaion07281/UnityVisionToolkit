using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Event raised when a scene load operation starts.
    /// </summary>
    public struct SceneLoadStartedEvent
    {
        public string SceneName;
        public SceneLoadStartedEvent(string sceneName) { SceneName = sceneName; }
    }

    /// <summary>
    /// Event raised when a scene load operation completes.
    /// </summary>
    public struct SceneLoadCompletedEvent
    {
        public string SceneName;
        public SceneLoadCompletedEvent(string sceneName) { SceneName = sceneName; }
    }

    /// <summary>
    /// Manages scene transitions with a fade-to-black overlay.
    /// Inherits from Singleton to persist across scenes.
    /// </summary>
    public class SceneLoader : Singleton<SceneLoader>
    {
        private CanvasGroup _canvasGroup;
        private bool _isLoading = false;

        /// <summary>
        /// Initializes the SceneLoader, creating the persistent overlay canvas.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // Create Canvas GameObject
            GameObject canvasObj = new GameObject("SceneLoaderCanvas");
            DontDestroyOnLoad(canvasObj);

            // Setup Canvas
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 9999; // Draw on top

            // Setup Canvas Scaler
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            // Setup Graphic Raycaster
            canvasObj.AddComponent<GraphicRaycaster>();

            // Setup Canvas Group
            _canvasGroup = canvasObj.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;

            // Create Overlay Image
            GameObject imageObj = new GameObject("Overlay");
            imageObj.transform.SetParent(canvasObj.transform, false);

            Image image = imageObj.AddComponent<Image>();
            image.color = Color.black;

            // Stretch Image to fill screen
            RectTransform rect = image.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        /// <summary>
        /// Loads a new scene with a fade transition.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        /// <param name="fadeDuration">The duration of the fade in/out effect.</param>
        public void LoadScene(string sceneName, float fadeDuration = 0.5f)
        {
            if (_isLoading)
            {
                Debug.LogWarning("[SceneLoader] Scene load already in progress. Ignoring request.");
                return;
            }
            _isLoading = true;
            StartCoroutine(LoadSceneRoutine(sceneName, fadeDuration));
        }

        /// <summary>
        /// Loads a new scene by build index with a fade transition.
        /// </summary>
        /// <param name="buildIndex">The build index of the scene to load.</param>
        /// <param name="fadeDuration">The duration of the fade in/out effect.</param>
        public void LoadScene(int buildIndex, float fadeDuration = 0.5f)
        {
            if (_isLoading)
            {
                Debug.LogWarning("[SceneLoader] Scene load already in progress. Ignoring request.");
                return;
            }

            if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogError($"[SceneLoader] Invalid build index: {buildIndex}. Must be between 0 and {SceneManager.sceneCountInBuildSettings - 1}.");
                return;
            }

            string scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            LoadScene(sceneName, fadeDuration);
        }

        private IEnumerator LoadSceneRoutine(string sceneName, float fadeDuration)
        {
            // Step 1: Raise Start Event
            EventBus.Raise(new SceneLoadStartedEvent(sceneName));

            // Step 2: Block Raycasts
            _canvasGroup.blocksRaycasts = true;

            // Step 3: Fade In (Alpha 0 -> 1)
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                yield return null;
            }
            _canvasGroup.alpha = 1f;

            // Step 4: Load Scene
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            // Wait until scene is fully loaded
            while (!op.isDone)
            {
                yield return null;
            }

            // Step 5: Fade Out (Alpha 1 -> 0)
            timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                yield return null;
            }
            _canvasGroup.alpha = 0f;

            // Step 6: Unblock Raycasts
            _canvasGroup.blocksRaycasts = false;

            // Step 7: Raise Complete Event
            EventBus.Raise(new SceneLoadCompletedEvent(sceneName));

            _isLoading = false;
        }
    }
}
