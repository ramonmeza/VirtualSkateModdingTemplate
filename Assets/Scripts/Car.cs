using UnityEngine;

namespace VirtualSkateMappingTools
{
    public class Car : MonoBehaviour
    {
        public float distance = 10.0f;
        public float speed = 10.0f;
        private Vector3 startPosition;

        private void Start()
        {
            startPosition = transform.position;
        }

        void FixedUpdate()
        {
            var dt = Time.fixedDeltaTime;

            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.forward, speed * dt);
            if (Vector3.Distance(transform.position, startPosition) > distance)
            {
                transform.position = startPosition;
            }
        }
    }
}
