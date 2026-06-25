using UnityEngine;
using UnityEngine.UI;
using UnityVisionToolkit.Runtime;
using System.Reflection;

namespace UnityVisionToolkit.Samples
{
    public class ObjectPoolBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            // 1. Create a Primitive Cube to serve as the prefab
            var prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            prefab.SetActive(false); // Hide the original
            prefab.name = "PooledCubePrefab";

            // 2. Setup the GameObjectPool
            var poolObj = new GameObject("ObjectPool");
            var pool = poolObj.AddComponent<GameObjectPool>();

            // Use reflection to set the private _prefab field
            var prefabField = typeof(GameObjectPool).GetField("_prefab", BindingFlags.NonPublic | BindingFlags.Instance);
            if (prefabField != null)
            {
                prefabField.SetValue(pool, prefab);
            }

            // 3. Setup the Sample Component
            var sample = gameObject.AddComponent<ObjectPoolSample>();
            sample.Pool = pool;

            // 4. Create UI
            var canvasObj = new GameObject("Canvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            // Spawn Button
            var spawnBtnObj = new GameObject("SpawnButton");
            spawnBtnObj.transform.SetParent(canvasObj.transform, false);
            var spawnRect = spawnBtnObj.AddComponent<RectTransform>();
            spawnRect.sizeDelta = new Vector2(200, 50);
            spawnRect.anchoredPosition = new Vector2(0, -200); // Bottom of screen

            var spawnImage = spawnBtnObj.AddComponent<Image>();
            spawnImage.color = Color.white;

            var spawnButton = spawnBtnObj.AddComponent<Button>();
            spawnButton.onClick.AddListener(() => sample.SpawnObject());

            var spawnTextObj = new GameObject("Text");
            spawnTextObj.transform.SetParent(spawnBtnObj.transform, false);
            var spawnTextRect = spawnTextObj.AddComponent<RectTransform>();
            spawnTextRect.sizeDelta = new Vector2(200, 50);

            var spawnText = spawnTextObj.AddComponent<Text>();
            spawnText.text = "Spawn Object";
            spawnText.alignment = TextAnchor.MiddleCenter;
            spawnText.color = Color.black;
            spawnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // Camera (Just in case one isn't present, so we can see the cubes)
            if (Camera.main == null)
            {
                var camObj = new GameObject("Main Camera");
                var cam = camObj.AddComponent<Camera>();
                cam.tag = "MainCamera";
                camObj.transform.position = new Vector3(0, 0, -10);
            }
        }
    }
}
