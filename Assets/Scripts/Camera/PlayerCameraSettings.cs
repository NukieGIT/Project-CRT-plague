using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Camera Settings", fileName = "PlayerCameraSettings")]
public class PlayerCameraSettings : ScriptableObject
{
    [SerializeField] private float mouseSensitivity = 1f;
    public float MouseSensitivity => mouseSensitivity;
}