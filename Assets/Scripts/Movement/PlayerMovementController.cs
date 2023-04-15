using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public partial class PlayerMovementController : MonoBehaviour, IPlayerMovement
    {
        public bool IsGrounded { get; private set; }
        public bool IsCrouching { get; private set; }

        public Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }
        
        public float FrictionCoefficient { 
            get => frictionCoefficient;
            set => frictionCoefficient = value;
        }
        
        public Vector3 Position => _rigidbody.position;
        
        // refs
        private IPlayerCamera _playerCameraController;
        private Rigidbody _rigidbody;
        private CapsuleCollider _capsuleCollider;
        private CharacterInput _characterInput;

        
        // private fields
        private readonly GroundChecker _groundChecker = new();
        private RaycastHit _hit;
        
        private void Awake()
        {
            _playerCameraController = GetComponent<IPlayerCamera>();
            _rigidbody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _characterInput = new CharacterInput();
        }
        
        private void OnEnable()
        {
            _characterInput.Humanoid.Enable();
            _characterInput.Humanoid.Jump.performed += Jump;
            _characterInput.Humanoid.Slide.started += SlideStart;
            _characterInput.Humanoid.Slide.canceled += SlideEnd;
        }

        private void OnDisable()
        {
            _characterInput.Humanoid.Disable();
            _characterInput.Humanoid.Jump.performed -= Jump;
            _characterInput.Humanoid.Slide.started -= SlideStart;
            _characterInput.Humanoid.Slide.canceled -= SlideEnd;
        }

        private void Update()
        {
            IsGrounded = _groundChecker.IsGrounded(transform, _capsuleCollider.bounds, extraHeight, out _hit);
        }

        private void FixedUpdate()
        {
            if (IsGrounded)
            {
                ApplyFriction();
            }

            if (IsGrounded && !IsSliding)
            {
                Move();
            }
            
        }

        private void ApplyFriction()
        {
            var velMag = _rigidbody.velocity.magnitude;
            if (velMag <= 0f) return;
            
            var frictionForceMag = frictionCoefficient * velMag;

            var counterForce = -_rigidbody.velocity * frictionForceMag / velMag;

            _rigidbody.AddForce(counterForce, ForceMode.VelocityChange);
        }
        

        private void Move()
        {
            var playerInput = _characterInput.Humanoid.Movement.ReadValue<Vector2>();
            var movementDirection = new Vector3(playerInput.x, 0, playerInput.y);
            var normal = _hit.normal;
            
            var cameraRotationVector = _playerCameraController.CameraRotationVector;
            cameraRotationVector = new Vector2(0, cameraRotationVector.y);
            var cameraRotation = Quaternion.Euler(cameraRotationVector);

            var desiredMovementDirection = cameraRotation * movementDirection;

            var velocityInDirection = Vector3.Dot(_rigidbody.velocity, desiredMovementDirection);
            
            var missingSpeed = acceleration;
            
            if (velocityInDirection + missingSpeed >= maxMovementSpeed)
            {
                missingSpeed = Mathf.Clamp(maxMovementSpeed - velocityInDirection, 0, acceleration);
            }

            var force = desiredMovementDirection * missingSpeed;
            force = Vector3.ProjectOnPlane(force, normal);
            
            _rigidbody.AddForce(force, ForceMode.VelocityChange);

        }

        private void OnDrawGizmosSelected()
        {
            
            if (!Application.isPlaying) return;
            Gizmos.color = IsGrounded ? Color.green : Color.red;

            var bounds = _capsuleCollider.bounds;
            
            var toCenter = Vector3.down * (bounds.extents.y / 2);
                
            Gizmos.DrawWireCube(bounds.center + toCenter + Vector3.down * extraHeight / 2,
                new Vector3(bounds.size.x,
                    bounds.extents.y + extraHeight,
                    bounds.size.z));
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_hit.point, _hit.point + _hit.normal * 5);
        }
        
        public void Move(Vector3 direction)
        {
            throw new System.NotImplementedException();
        }
        
        public void Teleport(Vector3 position)
        {
            _rigidbody.position = position;
        }
    }
}
