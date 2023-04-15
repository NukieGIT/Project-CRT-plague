using UnityEngine;

namespace Movement
{
    public partial class PlayerMovementController
    {
        [Header("Settings")]
        
        [SerializeField] private float maxMovementSpeed = 5f;
        [SerializeField] private float acceleration = 0.5f;
        
        [SerializeField] private float frictionCoefficient = 0.16f;
        
        [SerializeField] private float slideSpeed = 15f;
        [SerializeField] private float slideFrictionCoefficient = 0.07f;
        
        [SerializeField] private float jumpForce = 600f;
        [SerializeField] private float extraHeight = 0.01f;
    }
}