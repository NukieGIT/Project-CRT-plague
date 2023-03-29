using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovementInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private PlayerCameraController playerCameraController;

    private void Update()
    {
        text.text = $"Player position: {playerMovementController.transform.position}\n" +
                    $"Player velocity: {Math.Round(playerMovementController.Velocity.magnitude, 3)}\n" +
                    $"Player XZ velocity: {Math.Round(new Vector2(playerMovementController.Velocity.x, playerMovementController.Velocity.z).magnitude, 2)}\n" +
                    $"{playerMovementController.Velocity}\n" +
                    $"Player camera rotation: {playerCameraController.CameraRotationVector}";
    }
}
