using UnityEngine;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    public class ObjectPoolSample : MonoBehaviour
    {
        [SerializeField] private GameObjectPool _pool;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _pool != null)
            {
                var instance = _pool.Get();
                instance.transform.position = Random.insideUnitSphere * 5f;

                // Return to pool after 2 seconds
                StartCoroutine(ReturnToPoolRoutine(instance));
            }
        }

        private System.Collections.IEnumerator ReturnToPoolRoutine(GameObject instance)
        {
            yield return new WaitForSeconds(2f);
            _pool.Release(instance);
        }
    }
}