using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    public class IdleState : IState
    {
        private readonly Text _logText;
        public IdleState(Text logText) { _logText = logText; }
        public void OnEnter() { Debug.Log("Entered Idle State"); if(_logText) _logText.text = "Current State: IDLE"; }
        public void OnExit() { Debug.Log("Exited Idle State"); }
        public void OnUpdate() {}
        public void OnFixedUpdate() {}
    }

    public class WalkState : IState
    {
        private readonly Text _logText;
        public WalkState(Text logText) { _logText = logText; }
        public void OnEnter() { Debug.Log("Entered Walk State"); if(_logText) _logText.text = "Current State: WALK"; }
        public void OnExit() { Debug.Log("Exited Walk State"); }
        public void OnUpdate() {}
        public void OnFixedUpdate() {}
    }

    public class StateMachineSample : MonoBehaviour
    {
        private StateMachine _stateMachine;
        private Text _stateText;

        private void Start()
        {
            CreateUI();

            _stateMachine = new StateMachine();
            _stateMachine.ChangeState(new IdleState(_stateText));
        }

        private void Update()
        {
            _stateMachine?.Update();
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
            titleText.text = "StateMachine Sample";
            titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            titleText.fontSize = 24;
            titleText.alignment = TextAnchor.UpperCenter;
            var titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 1);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.offsetMin = new Vector2(0, -50);
            titleRect.offsetMax = new Vector2(0, 0);

            // State Text
            var stateObj = new GameObject("StateText");
            stateObj.transform.SetParent(canvasObj.transform, false);
            _stateText = stateObj.AddComponent<Text>();
            _stateText.text = "Current State: NONE";
            _stateText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _stateText.fontSize = 20;
            _stateText.alignment = TextAnchor.MiddleCenter;
            var stateRect = stateObj.GetComponent<RectTransform>();
            stateRect.anchorMin = new Vector2(0, 0.5f);
            stateRect.anchorMax = new Vector2(1, 0.5f);
            stateRect.offsetMin = new Vector2(0, 50);
            stateRect.offsetMax = new Vector2(0, 100);

            // Buttons
            CreateButton(canvasObj.transform, "Idle", new Vector2(-110, -50), () => _stateMachine.ChangeState(new IdleState(_stateText)));
            CreateButton(canvasObj.transform, "Walk", new Vector2(110, -50), () => _stateMachine.ChangeState(new WalkState(_stateText)));
        }

        private void CreateButton(Transform parent, string label, Vector2 position, UnityEngine.Events.UnityAction action)
        {
            var btnObj = new GameObject(label + "Button");
            btnObj.transform.SetParent(parent, false);
            var btnImage = btnObj.AddComponent<Image>();
            btnImage.color = Color.gray;
            var btn = btnObj.AddComponent<Button>();
            var btnRect = btnObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.5f);
            btnRect.anchorMax = new Vector2(0.5f, 0.5f);
            btnRect.sizeDelta = new Vector2(200, 50);
            btnRect.anchoredPosition = position;

            var btnTextObj = new GameObject("Text");
            btnTextObj.transform.SetParent(btnObj.transform, false);
            var btnText = btnTextObj.AddComponent<Text>();
            btnText.text = "Switch to " + label;
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