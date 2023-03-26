using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCameraController playerCameraController;
    [Space]
    [Header("Settings")]
    [SerializeField] private float maxMovementSpeed = 10f;
    [SerializeField] private float acceleration = 3f;
    [SerializeField] private float jumpForce = 100f;
    [SerializeField] private float extraHeight = 0.01f;

    public bool IsGrounded { get; private set; }
    
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    
    private CharacterInput _characterInput;
    
    private RaycastHit _hit;

    private void Awake()
    {
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
        // TODO: delegate it to another class pls
        var bounds = _capsuleCollider.bounds;
        IsGrounded = Physics.BoxCast(bounds.center,
            bounds.extents / 2,
            Vector3.down,
            out _hit,
            _rigidbody.rotation,
            bounds.extents.y / 2 + extraHeight);
    }

    private void FixedUpdate()
    {
        var playerInput = _characterInput.Humanoid.Movement.ReadValue<Vector2>();
        var movementDirection = new Vector3(playerInput.x, 0, playerInput.y);
        
        var cameraRotationVector = playerCameraController.CameraRotationVector;
        cameraRotationVector = new Vector2(0, cameraRotationVector.y);
        var cameraRotation = Quaternion.Euler(cameraRotationVector);
        
        var desiredMovementDirection = cameraRotation * movementDirection;
        
        var velocityInDirection = Vector3.Dot(_rigidbody.velocity, desiredMovementDirection);

        // you can strafe and get infinite speed lmao gotta fix this
        
        if (velocityInDirection < maxMovementSpeed)
        {
            _rigidbody.AddForce(desiredMovementDirection * acceleration, ForceMode.VelocityChange);
        }

        Debug.Log("_rigidbody.velocity.magnitude = " + _rigidbody.velocity.magnitude);
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
    }
}
