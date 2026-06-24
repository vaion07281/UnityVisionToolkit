using UnityEngine;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    public class UIPanelSample : UIPanel
    {
        public override void Show()
        {
            base.Show();
            Debug.Log("Sample Panel Shown");
        }

        public override void Hide()
        {
            base.Hide();
            Debug.Log("Sample Panel Hidden");
        }
    }

    public class UIManagerSample : MonoBehaviour
    {
        [SerializeField] private UIPanelSample _samplePanel;

        private void Start()
        {
            // Assuming UIManager is setup in the scene
            if (UIManager.Instance != null && _samplePanel != null)
            {
                UIManager.Instance.Push(_samplePanel);
            }
        }
    }
}