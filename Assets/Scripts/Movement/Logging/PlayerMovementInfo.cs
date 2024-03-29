using System;
using TMPro;
using UnityEngine;

namespace Movement.Logging
{
    public class PlayerMovementInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Movement.PlayerMovementController playerMovementController;
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
}
