using UnityEngine;

public interface IPlayerMovement
{
    bool IsGrounded { get; }
    bool IsSliding { get; }
    bool IsCrouching { get; }
    Vector3 Velocity { get; set; }
    Vector3 Position { get; }
    void Move(Vector3 direction);
    void Teleport(Vector3 position);
}
