using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Manages UI navigation using a stack-based system.
    /// Handles panel lifecycle events (Open, Close, Focus, LoseFocus).
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        private readonly Stack<UIPanel> _panelStack = new Stack<UIPanel>();
        private readonly Dictionary<Type, UIPanel> _panelMap = new Dictionary<Type, UIPanel>();

        /// <summary>
        /// Initializes the UIManager, finding and registering all child UIPanels.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            UIPanel[] panels = GetComponentsInChildren<UIPanel>(true);
            foreach (var panel in panels)
            {
                Type type = panel.GetType();
                if (_panelMap.ContainsKey(type))
                {
                    Debug.LogWarning($"[UIManager] Duplicate panel type found: {type.Name}. Skipping.");
                    continue;
                }

                _panelMap.Add(type, panel);
                panel.Init();
                panel.Close(); // Default to closed
            }
        }

        /// <summary>
        /// Pushes a panel of type T onto the navigation stack.
        /// </summary>
        /// <typeparam name="T">The type of UIPanel to open.</typeparam>
        public void Push<T>() where T : UIPanel
        {
            if (!_panelMap.TryGetValue(typeof(T), out UIPanel panel))
            {
                Debug.LogError($"[UIManager] Cannot push {typeof(T).Name}: Panel not found in children.");
                return;
            }

            // Prevent pushing if already on top
            if (_panelStack.Count > 0 && _panelStack.Peek() == panel)
            {
                return;
            }

            // Notify previous top
            if (_panelStack.Count > 0)
            {
                _panelStack.Peek().OnLoseFocus();
            }

            // Open and Focus new panel
            panel.Open();
            panel.OnFocus();
            _panelStack.Push(panel);
        }

        /// <summary>
        /// Pops the current top panel from the stack and closes it.
        /// Returns focus to the previous panel if one exists.
        /// </summary>
        public void Pop()
        {
            if (_panelStack.Count == 0)
            {
                Debug.LogWarning("[UIManager] UI Stack is empty, cannot Pop.");
                return;
            }

            // Close current
            UIPanel current = _panelStack.Pop();
            current.Close();

            // Focus previous
            if (_panelStack.Count > 0)
            {
                _panelStack.Peek().OnFocus();
            }
        }

        /// <summary>
        /// Retrieves the registered instance of a specific UIPanel type.
        /// </summary>
        /// <typeparam name="T">The panel type.</typeparam>
        /// <returns>The instance of the panel, or null if not found.</returns>
        public T GetPanel<T>() where T : UIPanel
        {
            if (_panelMap.TryGetValue(typeof(T), out UIPanel panel))
            {
                return panel as T;
            }
            return null;
        }
    }
}
