using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour, IPlayerMovement
{
    [Header("Settings")]
    [SerializeField] private float maxMovementSpeed = 5f;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private float frictionCoefficient = 0.16f;
    [SerializeField] private float slideSpeed = 15f;
    [SerializeField] private float jumpForce = 600f;
    [SerializeField] private float extraHeight = 0.01f;

    public bool IsGrounded { get; private set; }
    public bool IsSliding { get; private set; }
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

    private void SlideEnd(InputAction.CallbackContext obj)
    {
        IsSliding = false;
    }

    private void SlideStart(InputAction.CallbackContext obj)
    {
        if (!IsGrounded) return;
        
        IsSliding = true;

        var diff = Mathf.Max(slideSpeed - _rigidbody.velocity.magnitude, 0);
        
        var force = _rigidbody.velocity.normalized * diff;
        force.y = 0;
        
        _rigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    private void OnDisable()
    {
        _characterInput.Humanoid.Disable();
        _characterInput.Humanoid.Jump.performed -= Jump;
        _characterInput.Humanoid.Slide.started -= SlideStart;
        _characterInput.Humanoid.Slide.canceled -= SlideEnd;
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
