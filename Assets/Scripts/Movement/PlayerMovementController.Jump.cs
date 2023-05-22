using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public partial class PlayerMovementController
    {
        private void Jump(InputAction.CallbackContext ctx)
        {
            if (!IsGrounded) return;
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}