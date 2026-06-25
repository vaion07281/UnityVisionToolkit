using UnityEngine;
using UnityEngine.UI;

namespace UnityVisionToolkit.Samples
{
    public class StateMachineBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            // Create the sample component
            var sample = gameObject.AddComponent<StateMachineSample>();

            // Create Canvas
            var canvasObj = new GameObject("Canvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            // Create Walk Button
            var walkBtnObj = new GameObject("WalkButton");
            walkBtnObj.transform.SetParent(canvasObj.transform, false);
            var walkRect = walkBtnObj.AddComponent<RectTransform>();
            walkRect.sizeDelta = new Vector2(200, 50);
            walkRect.anchoredPosition = new Vector2(-110, 0);

            var walkImage = walkBtnObj.AddComponent<Image>();
            walkImage.color = Color.white;

            var walkButton = walkBtnObj.AddComponent<Button>();
            walkButton.onClick.AddListener(() => sample.SwitchToWalk());

            var walkTextObj = new GameObject("Text");
            walkTextObj.transform.SetParent(walkBtnObj.transform, false);
            var walkTextRect = walkTextObj.AddComponent<RectTransform>();
            walkTextRect.sizeDelta = new Vector2(200, 50);

            var walkText = walkTextObj.AddComponent<Text>();
            walkText.text = "Walk State";
            walkText.alignment = TextAnchor.MiddleCenter;
            walkText.color = Color.black;
            walkText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // Create Idle Button
            var idleBtnObj = new GameObject("IdleButton");
            idleBtnObj.transform.SetParent(canvasObj.transform, false);
            var idleRect = idleBtnObj.AddComponent<RectTransform>();
            idleRect.sizeDelta = new Vector2(200, 50);
            idleRect.anchoredPosition = new Vector2(110, 0);

            var idleImage = idleBtnObj.AddComponent<Image>();
            idleImage.color = Color.white;

            var idleButton = idleBtnObj.AddComponent<Button>();
            idleButton.onClick.AddListener(() => sample.SwitchToIdle());

            var idleTextObj = new GameObject("Text");
            idleTextObj.transform.SetParent(idleBtnObj.transform, false);
            var idleTextRect = idleTextObj.AddComponent<RectTransform>();
            idleTextRect.sizeDelta = new Vector2(200, 50);

            var idleText = idleTextObj.AddComponent<Text>();
            idleText.text = "Idle State";
            idleText.alignment = TextAnchor.MiddleCenter;
            idleText.color = Color.black;
            idleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // Create Status Text
            var statusObj = new GameObject("StatusText");
            statusObj.transform.SetParent(canvasObj.transform, false);
            var statusRect = statusObj.AddComponent<RectTransform>();
            statusRect.sizeDelta = new Vector2(400, 50);
            statusRect.anchoredPosition = new Vector2(0, -60);

            var statusText = statusObj.AddComponent<Text>();
            statusText.text = "Check console for state logs.";
            statusText.alignment = TextAnchor.MiddleCenter;
            statusText.color = Color.white;
            statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }
    }
}
