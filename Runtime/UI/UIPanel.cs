using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Abstract base class for all UI panels managed by the UIManager.
    /// </summary>
    public abstract class UIPanel : MonoBehaviour
    {
        /// <summary>
        /// Gets a value indicating whether this panel is currently open (active).
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Called by UIManager during Awake initialization.
        /// Use this for one-time setup (e.g., caching components).
        /// </summary>
        public virtual void Init() { }

        /// <summary>
        /// Opens the panel, setting the GameObject active and updating state.
        /// </summary>
        public virtual void Open()
        {
            IsOpen = true;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Closes the panel, setting the GameObject inactive and updating state.
        /// </summary>
        public virtual void Close()
        {
            IsOpen = false;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Called when this panel becomes the top of the navigation stack.
        /// </summary>
        public virtual void OnFocus() { }

        /// <summary>
        /// Called when a new panel is pushed on top of this one, causing it to lose focus.
        /// </summary>
        public virtual void OnLoseFocus() { }
    }
}
