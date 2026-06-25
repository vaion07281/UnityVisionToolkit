using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    public class UIPanelSample : UIPanel
    {
        public override void Show()
        {
            base.Show();
            gameObject.SetActive(true);
            Debug.Log("Sample Panel Shown");
        }

        public override void Hide()
        {
            base.Hide();
            gameObject.SetActive(false);
            Debug.Log("Sample Panel Hidden");
        }
    }

    public class UIManagerSample : MonoBehaviour
    {
        private UIPanelSample _samplePanel;

        private void Start()
        {
            CreateUI();

            // Auto-setup UIManager if missing
            if (UIManager.Instance == null)
            {
                var uiManagerObj = new GameObject("UIManager");
                uiManagerObj.AddComponent<UIManager>();
            }

            // Register the panel manually for the sample since it was dynamically created
            // (UIManager usually finds them via GetComponentsInChildren in Awake)
            UIManager.Instance.Push(_samplePanel);
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
            titleText.text = "UIManager Sample";
            titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            titleText.fontSize = 24;
            titleText.alignment = TextAnchor.UpperCenter;
            var titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 1);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.offsetMin = new Vector2(0, -50);
            titleRect.offsetMax = new Vector2(0, 0);

            // Create Panel
            var panelObj = new GameObject("SamplePanel");
            panelObj.transform.SetParent(canvasObj.transform, false);
            var panelImage = panelObj.AddComponent<Image>();
            panelImage.color = new Color(0.2f, 0.2f, 0.8f, 0.8f);
            var panelRect = panelObj.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.2f, 0.2f);
            panelRect.anchorMax = new Vector2(0.8f, 0.8f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            var panelTextObj = new GameObject("PanelText");
            panelTextObj.transform.SetParent(panelObj.transform, false);
            var panelText = panelTextObj.AddComponent<Text>();
            panelText.text = "This is a UIPanel!\nUse UIManager to Push/Pop.";
            panelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            panelText.fontSize = 20;
            panelText.color = Color.white;
            panelText.alignment = TextAnchor.MiddleCenter;
            var panelTextRect = panelTextObj.GetComponent<RectTransform>();
            panelTextRect.anchorMin = Vector2.zero;
            panelTextRect.anchorMax = Vector2.one;
            panelTextRect.sizeDelta = Vector2.zero;

            _samplePanel = panelObj.AddComponent<UIPanelSample>();
            panelObj.SetActive(false); // hide initially

            // Buttons
            CreateButton(canvasObj.transform, "Push Panel", new Vector2(-110, 50), () => UIManager.Instance.Push(_samplePanel));
            CreateButton(canvasObj.transform, "Pop Panel", new Vector2(110, 50), () => UIManager.Instance.Pop());
        }

        private void CreateButton(Transform parent, string label, Vector2 position, UnityEngine.Events.UnityAction action)
        {
            var btnObj = new GameObject(label.Replace(" ", "") + "Button");
            btnObj.transform.SetParent(parent, false);
            var btnImage = btnObj.AddComponent<Image>();
            btnImage.color = Color.gray;
            var btn = btnObj.AddComponent<Button>();
            var btnRect = btnObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.1f);
            btnRect.anchorMax = new Vector2(0.5f, 0.1f);
            btnRect.sizeDelta = new Vector2(200, 50);
            btnRect.anchoredPosition = position;

            var btnTextObj = new GameObject("Text");
            btnTextObj.transform.SetParent(btnObj.transform, false);
            var btnText = btnTextObj.AddComponent<Text>();
            btnText.text = label;
            btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            btnText.color = Color.white;
            btnText.alignment = TextAnchor.MiddleCenter;
            var btnTextRect = btnTextObj.GetComponent<RectTransform>();
            btnTextRect.anchorMin = Vector2.zero;
            btnTextRect.anchorMax = Vector2.one;
            btnTextRect.sizeDelta = Vector2.zero;

            btn.onClick.AddListener(action);
        }
    }
}