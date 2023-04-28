using UnityEngine;

namespace PlayerCamera
{
    public class PlayerCameraController : MonoBehaviour, IPlayerCamera
    {
    
        [Header("References")]
        [SerializeField] private Transform cameraTargetPosition;
        [SerializeField] private Transform cameraTransform;
        [Space]
    
        [Header("Settings")]
        [SerializeField] private PlayerCameraSettings playerCameraSettings;
    
        public PlayerCameraSettings PlayerCameraSettings => playerCameraSettings;
    
        private Vector2 _cameraRotationVector = Vector2.zero;
        public Vector2 CameraRotationVector => _cameraRotationVector;

        private CharacterInput _characterInput;

        private void Awake()
        {
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

            var mouseDelta = _characterInput.HumanoidCamera.Camera.ReadValue<Vector2>();
            mouseDelta *= PlayerCameraSettings.MouseSensitivity;
        
            _cameraRotationVector += new Vector2(-mouseDelta.y, mouseDelta.x);
            _cameraRotationVector.x = Mathf.Clamp(_cameraRotationVector.x, -90f, 90f);
        
            cameraTransform.transform.rotation = Quaternion.Euler(CameraRotationVector);
            cameraTransform.transform.position = cameraTargetPosition.position;
        }
    
        public void LookAt(Transform target)
        {
            throw new System.NotImplementedException();
        }

        public void LookAt(Vector3 target)
        {
            throw new System.NotImplementedException();
        }

    }
}