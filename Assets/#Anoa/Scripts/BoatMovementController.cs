using UnityEngine;

namespace Anoa
{
    public class BoatMovementController : MonoBehaviour
    {
        [SerializeField] protected float floatMoveSpeed = 8f;
        protected int intMoveDirection;

        // Tambah variable untuk cek status memancing
        protected bool boolCanMove = true;

        public void StartMoveLeft()
        {
            if (boolCanMove)
            {
                intMoveDirection = -1;
            }
        }

        public void StartMoveRight()
        {
            if (boolCanMove)
            {
                intMoveDirection = 1;
            }
        }

        public void StopMove()
        {
            intMoveDirection = 0;
        }

        // Function untuk set bisa gerak atau tidak
        public void SetCanMove(bool canMove)
        {
            boolCanMove = canMove;
            if (!canMove)
            {
                StopMove(); // Stop movement jika tidak bisa gerak
            }
        }

        protected void Update()
        {
            HandleMovement();
        }

        protected void HandleMovement()
        {
            if (boolCanMove)
            {
                transform.Translate(Vector3.right * intMoveDirection * floatMoveSpeed * Time.deltaTime);
            }
        }
    }
}