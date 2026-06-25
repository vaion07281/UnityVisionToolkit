using UnityEngine;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    public class UIPanelSample : UIPanel
    {
        public override void Open()
        {
            base.Open();
            Debug.Log("Sample Panel Opened");
        }

        public override void Close()
        {
            base.Close();
            Debug.Log("Sample Panel Closed");
        }
    }

    public class UIManagerSample : MonoBehaviour
    {
        [SerializeField] private UIPanelSample _samplePanel;

        private void Start()
        {
            // The bootstrap script now handles initial setup and interaction,
            // but this is kept as an example of programmatic access.
            if (UIManager.Instance != null && _samplePanel != null)
            {
                // Note: Using the generic Push method which is standard for UIManager
                UIManager.Instance.Push<UIPanelSample>();
            }
        }
    }
}
