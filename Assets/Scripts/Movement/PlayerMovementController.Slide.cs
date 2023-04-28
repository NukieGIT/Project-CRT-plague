using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public partial class PlayerMovementController
    {
        public bool IsSliding { get; private set; }

        private float _originalFrictionCoefficient;
        
        private void SlideEnd(InputAction.CallbackContext obj)
        {
            IsSliding = false;
            frictionCoefficient = _originalFrictionCoefficient;
        }

        private void SlideStart(InputAction.CallbackContext obj)
        {
            if (!IsGrounded) return;

            IsSliding = true;

            _originalFrictionCoefficient = frictionCoefficient;
            frictionCoefficient = slideFrictionCoefficient;
            
            var diff = Mathf.Max(slideSpeed - _rigidbody.velocity.magnitude, 0);

            var force = _rigidbody.velocity.normalized * diff;
            force.y = 0;

            _rigidbody.AddForce(force, ForceMode.VelocityChange);
        }
    }
}