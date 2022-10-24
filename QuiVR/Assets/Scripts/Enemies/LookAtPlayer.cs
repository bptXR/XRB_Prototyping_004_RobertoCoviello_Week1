using UnityEngine;

namespace Enemies
{
    public class LookAtPlayer : MonoBehaviour
    {
        [SerializeField] private Transform camera;

        private void Update()
        {
            transform.LookAt(transform.position + camera.forward);
        }
    }
}