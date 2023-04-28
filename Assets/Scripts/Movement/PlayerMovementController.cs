using PlayerCamera;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerMovementController : MonoBehaviour, IPlayerMovement
    {
        [Header("Settings")]
        [SerializeField] private float maxMovementSpeed = 5f;
        [SerializeField] private float acceleration = 0.5f;
        [SerializeField] private float jumpForce = 600f;
        [SerializeField] private float extraHeight = 0.01f;

        public bool IsGrounded { get; private set; }
        public Vector3 Velocity => _rigidbody.velocity;
        public Vector3 Position => _rigidbody.position;

    
        private IPlayerCamera _playerCameraController;
        private Rigidbody _rigidbody;
        private CapsuleCollider _capsuleCollider;
        private readonly GroundChecker _groundChecker = new();
        private CharacterInput _characterInput;
    
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
        }

        private void OnDisable()
        {
            _characterInput.Humanoid.Disable();
            _characterInput.Humanoid.Jump.performed -= Jump;
        }
    
        private void Jump(InputAction.CallbackContext ctx)
        {
            if (!IsGrounded) return;
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void Update()
        {
            IsGrounded = _groundChecker.IsGrounded(transform, _capsuleCollider.bounds, extraHeight, out _hit);
        }

        private void FixedUpdate()
        {
            var playerInput = _characterInput.Humanoid.Movement.ReadValue<Vector2>();
            var movementDirection = new Vector3(playerInput.x, 0, playerInput.y);
        
            var cameraRotationVector = _playerCameraController.CameraRotationVector;
            cameraRotationVector = new Vector2(0, cameraRotationVector.y);
            var cameraRotation = Quaternion.Euler(cameraRotationVector);
        
            var desiredMovementDirection = cameraRotation * movementDirection;
        
            var velocityInDirection = Vector3.Dot(_rigidbody.velocity, desiredMovementDirection);

            _rigidbody.velocity =
                new Vector3(desiredMovementDirection.x * maxMovementSpeed, _rigidbody.velocity.y,
                    desiredMovementDirection.z * maxMovementSpeed);

            // you can strafe and get infinite speed lmao gotta fix this

            // if (velocityInDirection < maxMovementSpeed)
            // {
            //     _rigidbody.AddForce(desiredMovementDirection * acceleration, ForceMode.VelocityChange);
            // }
        }

        private void OnDrawGizmosSelected()
        {
        
            // idk how to delegate gizmos to another class cause it just doesn't work
        
            if (!Application.isPlaying) return;
            Gizmos.color = IsGrounded ? Color.green : Color.red;

            var bounds = _capsuleCollider.bounds;
        
            var toCenter = Vector3.down * (bounds.extents.y / 2);
            
            Gizmos.DrawWireCube(bounds.center + toCenter + Vector3.down * extraHeight / 2,
                new Vector3(bounds.size.x,
                    bounds.extents.y + extraHeight,
                    bounds.size.z));
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
