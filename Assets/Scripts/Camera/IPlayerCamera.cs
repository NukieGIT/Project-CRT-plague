using UnityEngine;

public interface IPlayerCamera
{
    Vector2 CameraRotationVector { get; }
    PlayerCameraSettings PlayerCameraSettings { get; }
    void LookAt(Transform target);
    void LookAt(Vector3 target);
}