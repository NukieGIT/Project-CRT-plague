using System;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

[CreateAssetMenu(menuName = "Player/Player Camera Settings", fileName = "PlayerCameraSettings")]
public class PlayerCameraSettings : ScriptableObject
{
    [SerializeField] private float mouseSensitivity = 1f;
    public float MouseSensitivity => mouseSensitivity;
}