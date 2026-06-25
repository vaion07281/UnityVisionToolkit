using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    public class ObjectPoolSample : MonoBehaviour
    {
        [SerializeField] private GameObjectPool _pool;
        private Text _statusText;
        private int _activeObjects = 0;

        private void Start()
        {
            CreateUI();

            // Set up pool if not assigned
            if (_pool == null)
            {
                var poolObj = new GameObject("SamplePool");
                _pool = poolObj.AddComponent<GameObjectPool>();

                var prefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                prefab.transform.localScale = Vector3.one * 0.5f;
                prefab.name = "PooledSphere";
                prefab.SetActive(false);

                _pool.Prefab = prefab;
                _pool.DefaultCapacity = 10;
                _pool.MaxSize = 50;
            }
        }

        private void SpawnObject()
        {
            if (_pool != null)
            {
                var instance = _pool.Get();
                instance.transform.position = Random.insideUnitSphere * 3f + Vector3.forward * 5f;
                _activeObjects++;
                UpdateStatus();

                // Return to pool after 2 seconds
                StartCoroutine(ReturnToPoolRoutine(instance));
            }
        }

        private System.Collections.IEnumerator ReturnToPoolRoutine(GameObject instance)
        {
            yield return new WaitForSeconds(2f);
            _pool.Release(instance);
            _activeObjects--;
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (_statusText != null)
            {
                _statusText.text = $"Active Pooled Objects: {_activeObjects}";
            }
        }

        private void CreateUI()
        {
            // EventSystem
            if (FindObjectOfType<EventSystem>() == null)
            {
                var eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }

            // Canvas
            var canvasObj = new GameObject("Canvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            // Title Text
            var titleObj = new GameObject("TitleText");
            titleObj.transform.SetParent(canvasObj.transform, false);
            var titleText = titleObj.AddComponent<Text>();
            titleText.text = "ObjectPool Sample";
            titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            titleText.fontSize = 24;
            titleText.alignment = TextAnchor.UpperCenter;
            var titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 1);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.offsetMin = new Vector2(0, -50);
            titleRect.offsetMax = new Vector2(0, 0);

            // Status Text
            var statusObj = new GameObject("StatusText");
            statusObj.transform.SetParent(canvasObj.transform, false);
            _statusText = statusObj.AddComponent<Text>();
            _statusText.text = "Active Pooled Objects: 0";
            _statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _statusText.fontSize = 20;
            _statusText.alignment = TextAnchor.UpperCenter;
            var statusRect = statusObj.GetComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0, 1);
            statusRect.anchorMax = new Vector2(1, 1);
            statusRect.offsetMin = new Vector2(0, -100);
            statusRect.offsetMax = new Vector2(0, -50);

            // Button
            var btnObj = new GameObject("SpawnButton");
            btnObj.transform.SetParent(canvasObj.transform, false);
            var btnImage = btnObj.AddComponent<Image>();
            btnImage.color = Color.gray;
            var btn = btnObj.AddComponent<Button>();
            var btnRect = btnObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.1f);
            btnRect.anchorMax = new Vector2(0.5f, 0.1f);
            btnRect.sizeDelta = new Vector2(200, 50);
            btnRect.anchoredPosition = new Vector2(0, 50);

            var btnTextObj = new GameObject("Text");
            btnTextObj.transform.SetParent(btnObj.transform, false);
            var btnText = btnTextObj.AddComponent<Text>();
            btnText.text = "Spawn Object";
            btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            btnText.color = Color.white;
            btnText.alignment = TextAnchor.MiddleCenter;
            var btnTextRect = btnTextObj.GetComponent<RectTransform>();
            btnTextRect.anchorMin = Vector2.zero;
            btnTextRect.anchorMax = Vector2.one;
            btnTextRect.sizeDelta = Vector2.zero;

            btn.onClick.AddListener(SpawnObject);
        }
    }
}