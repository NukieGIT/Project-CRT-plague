using UnityEngine;

public interface IPlayerMovement
{
    bool IsGrounded { get; }
    Vector3 Velocity { get; }
    Vector3 Position { get; }
    void Move(Vector3 direction);
    void Teleport(Vector3 position);
}