using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private Transform cameraTargetPosition;

    [CanBeNull] private Camera _playerCamera;
    private CharacterInput _characterInput;
    private Vector2 _cameraRotation = Vector2.zero;
    // TODO: add some kind of shaking effect to the camera
    private Vector2 _cameraOffset = Vector2.zero;

    private void Awake()
    {
        _playerCamera = GetComponent<Camera>();
        _characterInput = new CharacterInput();
    }

    private void OnEnable()
    {
        _characterInput.Enable();
    }

    private void OnDisable()
    {
        _characterInput.Disable();
    }

    private void LateUpdate()
    {

        var mouseDelta = _characterInput.HumanoidCamera.Camera.ReadValue<Vector2>() * mouseSensitivity;
        
        _cameraRotation += new Vector2(-mouseDelta.y, mouseDelta.x);
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, -90f, 90f);
        
        _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation + _cameraOffset);
        _playerCamera.transform.position = cameraTargetPosition.position;
    }

}