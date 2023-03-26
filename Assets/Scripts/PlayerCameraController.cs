using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private Transform cameraTargetPosition;

    private Camera _playerCamera;
    private CharacterInput _characterInput;
    
    private Vector2 _cameraRotationVector = Vector2.zero;
    
    // TODO: add some kind of shaking effect to the camera
    private Vector2 _cameraOffset = Vector2.zero;
    
    public Vector2 CameraRotationVector => _cameraRotationVector;

    private void Awake()
    {
        _playerCamera = GetComponent<Camera>();
        _characterInput = new CharacterInput();
    }

    private void OnEnable()
    {
        _characterInput.HumanoidCamera.Enable();
    }

    private void OnDisable()
    {
        _characterInput.HumanoidCamera.Disable();
    }

    private void LateUpdate()
    {

        var mouseDelta = _characterInput.HumanoidCamera.Camera.ReadValue<Vector2>() * mouseSensitivity;
        
        _cameraRotationVector += new Vector2(-mouseDelta.y, mouseDelta.x);
        _cameraRotationVector.x = Mathf.Clamp(_cameraRotationVector.x, -90f, 90f);
        
        _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotationVector + _cameraOffset);
        _playerCamera.transform.position = cameraTargetPosition.position;
    }

}