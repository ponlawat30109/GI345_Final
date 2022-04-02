using UnityEngine;

namespace InventorySystem
{
    public class RotateFaceTarget : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void Start()
        {
            if (target == null)
            {
                if (Camera.main is { }) target = Camera.main.transform;
            }
        }

        void Update()
        {
            this.transform.LookAt(target.transform, transform.up);
        }
    }
}
