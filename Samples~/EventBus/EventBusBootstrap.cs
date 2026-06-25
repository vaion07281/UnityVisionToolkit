using UnityEngine;
using UnityEngine.UI;

namespace UnityVisionToolkit.Samples
{
    public class EventBusBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            // Create the sample component
            var sample = gameObject.AddComponent<EventBusSample>();

            // Create Canvas
            var canvasObj = new GameObject("Canvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            // Create Button
            var buttonObj = new GameObject("RaiseEventButton");
            buttonObj.transform.SetParent(canvasObj.transform, false);
            var rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200, 50);
            rectTransform.anchoredPosition = new Vector2(0, 0);

            var buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = Color.white;

            var button = buttonObj.AddComponent<Button>();
            button.onClick.AddListener(() => sample.RaiseEvent());

            // Create Text
            var textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            var textRect = textObj.AddComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(200, 50);

            var text = textObj.AddComponent<Text>();
            text.text = "Raise Event";
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.black;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // Create Status Text
            var statusObj = new GameObject("StatusText");
            statusObj.transform.SetParent(canvasObj.transform, false);
            var statusRect = statusObj.AddComponent<RectTransform>();
            statusRect.sizeDelta = new Vector2(400, 50);
            statusRect.anchoredPosition = new Vector2(0, -60);

            var statusText = statusObj.AddComponent<Text>();
            statusText.text = "Check console for event logs.";
            statusText.alignment = TextAnchor.MiddleCenter;
            statusText.color = Color.white;
            statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }
    }
}
