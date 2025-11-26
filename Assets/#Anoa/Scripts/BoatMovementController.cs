using UnityEngine;

namespace Anoa
{
    public class BoatMovementController : MonoBehaviour
    {
        [SerializeField] protected float floatMoveSpeed = 8f;
        protected int intMoveDirection;

        protected Rigidbody2D rb;
        protected bool boolCanMove = true;
        protected bool boolIsTouchingBoundary = false;

        // TAMBAHAN: Variabel batas posisi Kiri dan Kanan (sesuaikan di Inspector)
        [SerializeField] protected float floatDockLimitX = -2.0f;
        [SerializeField] protected float floatWallLimitX = 13.0f;

        protected void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D missing on Boat!");
            }
        }

        public void StartMoveLeft()
        {
            if (boolCanMove)
            {
                intMoveDirection = 1;
            }
        }

        public void StartMoveRight()
        {
            if (boolCanMove)
            {
                intMoveDirection = -1;
            }
        }

        public void StopMove()
        {
            intMoveDirection = 0;
        }

        public void SetCanMove(bool canMove)
        {
            boolCanMove = canMove;
            if (!canMove)
            {
                StopMove();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }
        }

        protected void FixedUpdate()
        {
            HandleMovement();
        }

        protected void HandleMovement()
        {
            if (!boolCanMove || rb == null) return;

            if (boolIsTouchingBoundary)
            {
                // Kondisi 1: Berhenti jika menempel di Kiri dan mencoba bergerak ke Kiri
                if (intMoveDirection < 0 && transform.position.x < floatDockLimitX)
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                    return;
                }

                // Kondisi 2: Berhenti jika menempel di Kanan dan mencoba bergerak ke Kanan
                if (intMoveDirection > 0 && transform.position.x > floatWallLimitX)
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                    return;
                }
            }

            float targetVelocityX = intMoveDirection * floatMoveSpeed;
            rb.linearVelocity = new Vector2(targetVelocityX, rb.linearVelocity.y);
        }

        protected void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                boolIsTouchingBoundary = true;
            }
        }

        protected void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                boolIsTouchingBoundary = false;
            }
        }
    }
}