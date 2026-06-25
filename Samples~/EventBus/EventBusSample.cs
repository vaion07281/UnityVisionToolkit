using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    public struct PlayerHealthChangedEvent
    {
        public int NewHealth;
    }

    public class EventBusSample : MonoBehaviour
    {
        private Text _logText;
        private int _currentHealth = 100;

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
            CreateUI();
        }

        private void OnHealthChanged(PlayerHealthChangedEvent evt)
        {
            string msg = $"Health changed to: {evt.NewHealth}";
            Debug.Log(msg);
            if (_logText != null)
            {
                _logText.text = msg + "\n" + _logText.text;
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
            titleText.text = "EventBus Sample";
            titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            titleText.fontSize = 24;
            titleText.alignment = TextAnchor.UpperCenter;
            var titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 1);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.offsetMin = new Vector2(0, -50);
            titleRect.offsetMax = new Vector2(0, 0);

            // Button
            var btnObj = new GameObject("PublishButton");
            btnObj.transform.SetParent(canvasObj.transform, false);
            var btnImage = btnObj.AddComponent<Image>();
            btnImage.color = Color.gray;
            var btn = btnObj.AddComponent<Button>();
            var btnRect = btnObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.5f);
            btnRect.anchorMax = new Vector2(0.5f, 0.5f);
            btnRect.sizeDelta = new Vector2(200, 50);
            btnRect.anchoredPosition = new Vector2(0, 50);

            var btnTextObj = new GameObject("Text");
            btnTextObj.transform.SetParent(btnObj.transform, false);
            var btnText = btnTextObj.AddComponent<Text>();
            btnText.text = "Take 10 Damage";
            btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            btnText.color = Color.white;
            btnText.alignment = TextAnchor.MiddleCenter;
            var btnTextRect = btnTextObj.GetComponent<RectTransform>();
            btnTextRect.anchorMin = Vector2.zero;
            btnTextRect.anchorMax = Vector2.one;
            btnTextRect.sizeDelta = Vector2.zero;

            btn.onClick.AddListener(() =>
            {
                _currentHealth -= 10;
                EventBus<PlayerHealthChangedEvent>.Raise(new PlayerHealthChangedEvent { NewHealth = _currentHealth });
            });

            // Log Text
            var logObj = new GameObject("LogText");
            logObj.transform.SetParent(canvasObj.transform, false);
            _logText = logObj.AddComponent<Text>();
            _logText.text = "Event logs:\n";
            _logText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _logText.fontSize = 18;
            _logText.alignment = TextAnchor.UpperCenter;
            var logRect = logObj.GetComponent<RectTransform>();
            logRect.anchorMin = new Vector2(0, 0);
            logRect.anchorMax = new Vector2(1, 0.5f);
            logRect.offsetMin = new Vector2(20, 20);
            logRect.offsetMax = new Vector2(-20, -50);
        }
    }
}