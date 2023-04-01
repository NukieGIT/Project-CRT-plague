using UnityEngine;

public interface IPlayerMovement
{
    bool IsGrounded { get; }
    Vector3 Velocity { get; set; }
    float FrictionCoefficient { get; set; }
    Vector3 Position { get; }
    void Move(Vector3 direction);
    void Teleport(Vector3 position);
}