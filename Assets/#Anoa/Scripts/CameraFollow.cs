using UnityEngine;

namespace Anoa
{

    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float smoothSpeed = 0.125f;
        public Vector3 offset = new Vector3(0f, 0f, -10f);
        
        public float minCameraX = 0f;

        public float maxCameraX = 3.4f;

        void FixedUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, offset.z);

            float clampedX = Mathf.Clamp(desiredPosition.x, minCameraX, maxCameraX);
            desiredPosition = new Vector3(clampedX, desiredPosition.y, desiredPosition.z);

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }
    }
}
