using UnityEngine;

namespace Movement
{
    public interface IPlayerMovement
    {
        bool IsGrounded { get; }
        bool IsSliding { get; }
        bool IsCrouching { get; }
        Vector3 Velocity { get; set; }
        Vector3 Position { get; }
        
        /// <summary>
        /// Defines the players frictions coefficient.
        /// </summary>
        float FrictionCoefficient { get; set; }
        
        /// <summary>
        /// Moves the player in the given direction.
        /// </summary>
        /// <param name="direction">Direction to which the player should move</param>
        void Move(Vector3 direction);
        
        /// <summary>
        /// Teleports the player to the specified position.
        /// </summary>
        /// <param name="position">Position at which the player will be placed at</param>
        void Teleport(Vector3 position);
    }
}
