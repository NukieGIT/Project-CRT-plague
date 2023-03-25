using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class PlayerCameraController : MonoBehaviour
{
    private Camera playerCamera;
    [SerializeField] private float mouseSensitivity = 0.1f;

    private CharacterInput _characterInput;
    private Vector2 _cameraRotation = Vector2.zero;

    private void Awake()
    {
        playerCamera = GetComponent<Camera>();
        _characterInput = new CharacterInput();
    }

    private void OnEnable()
    {
        _characterInput.HumanoidCamera.Camera.performed += MouseInput;
        _characterInput.Enable();
    }

    private void OnDisable()
    {
        _characterInput.HumanoidCamera.Camera.performed -= MouseInput;
        _characterInput.Disable();
    }

    private void LateUpdate()
    {
        playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation);
    }

    private void MouseInput(InputAction.CallbackContext ctx)
    {
        var _mouseDelta = ctx.ReadValue<Vector2>() * mouseSensitivity;
        
        _cameraRotation += new Vector2(-_mouseDelta.y, _mouseDelta.x);
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, -90f, 90f);
    }
}