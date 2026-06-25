using UnityEngine;
using UnityEngine.UI;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    public class UIManagerBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            // 1. Create Canvas (Parent for UI Manager)
            var canvasObj = new GameObject("Canvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            // 2. Create UI Manager Object (inactive so Awake doesn't run before children exist)
            var uiManagerObj = new GameObject("UIManager");
            uiManagerObj.transform.SetParent(canvasObj.transform, false);
            uiManagerObj.SetActive(false);
            var uiManager = uiManagerObj.AddComponent<UIManager>();

            // 3. Create Sample Panel as child of UIManager
            var panelObj = new GameObject("SamplePanel");
            panelObj.transform.SetParent(uiManagerObj.transform, false);
            var panelRect = panelObj.AddComponent<RectTransform>();
            panelRect.sizeDelta = new Vector2(400, 300);

            var panelImage = panelObj.AddComponent<Image>();
            panelImage.color = new Color(0.2f, 0.2f, 0.2f, 1f); // Dark gray

            // Add the UIPanel component
            var samplePanel = panelObj.AddComponent<UIPanelSample>();

            // Panel Text
            var panelTextObj = new GameObject("PanelText");
            panelTextObj.transform.SetParent(panelObj.transform, false);
            var panelTextRect = panelTextObj.AddComponent<RectTransform>();
            panelTextRect.sizeDelta = new Vector2(400, 50);
            panelTextRect.anchoredPosition = new Vector2(0, 100);

            var panelText = panelTextObj.AddComponent<Text>();
            panelText.text = "This is a Sample Panel!";
            panelText.alignment = TextAnchor.MiddleCenter;
            panelText.color = Color.white;
            panelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // Pop Button on the Panel
            var popBtnObj = new GameObject("PopButton");
            popBtnObj.transform.SetParent(panelObj.transform, false);
            var popRect = popBtnObj.AddComponent<RectTransform>();
            popRect.sizeDelta = new Vector2(150, 40);
            popRect.anchoredPosition = new Vector2(0, -50);

            var popImage = popBtnObj.AddComponent<Image>();
            popImage.color = Color.red;

            var popButton = popBtnObj.AddComponent<Button>();
            popButton.onClick.AddListener(() => UIManager.Instance.Pop());

            var popTextObj = new GameObject("Text");
            popTextObj.transform.SetParent(popBtnObj.transform, false);
            var popTextRect = popTextObj.AddComponent<RectTransform>();
            popTextRect.sizeDelta = new Vector2(150, 40);

            var popText = popTextObj.AddComponent<Text>();
            popText.text = "Close (Pop)";
            popText.alignment = TextAnchor.MiddleCenter;
            popText.color = Color.white;
            popText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // 4. Activate UIManager so it initializes with children
            uiManagerObj.SetActive(true);

            // 5. Create Push Button (Global button to open the panel)
            var pushBtnObj = new GameObject("PushButton");
            pushBtnObj.transform.SetParent(canvasObj.transform, false);
            var pushRect = pushBtnObj.AddComponent<RectTransform>();
            pushRect.sizeDelta = new Vector2(200, 50);
            pushRect.anchoredPosition = new Vector2(0, -200);

            var pushImage = pushBtnObj.AddComponent<Image>();
            pushImage.color = Color.green;

            var pushButton = pushBtnObj.AddComponent<Button>();
            pushButton.onClick.AddListener(() => UIManager.Instance.Push<UIPanelSample>());

            var pushTextObj = new GameObject("Text");
            pushTextObj.transform.SetParent(pushBtnObj.transform, false);
            var pushTextRect = pushTextObj.AddComponent<RectTransform>();
            pushTextRect.sizeDelta = new Vector2(200, 50);

            var pushText = pushTextObj.AddComponent<Text>();
            pushText.text = "Open Panel (Push)";
            pushText.alignment = TextAnchor.MiddleCenter;
            pushText.color = Color.white;
            pushText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // Note: UIManagerSample is technically no longer needed if we wire it directly,
            // but we can add it to the scene to maintain consistency if desired.
            // In this setup, the bootstrap replaces the need for UIManagerSample.cs's Start logic.
        }
    }
}
