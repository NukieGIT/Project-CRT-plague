using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float mouseSensitivity = 2f;
    private Vector2 _cameraRotation;
    private Vector2 _mouseDelta;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void MouseInput(InputAction.CallbackContext ctx)
    {
        _mouseDelta = Mouse.current.delta.ReadValue();
        _cameraRotation += new Vector2(-_mouseDelta.y * mouseSensitivity, _mouseDelta.x * mouseSensitivity);
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, -90f, 90f);

        camera.transform.rotation = Quaternion.Euler(_cameraRotation);
    }
}