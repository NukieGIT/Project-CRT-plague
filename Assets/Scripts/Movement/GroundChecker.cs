using UnityEngine;

namespace Movement
{
    public class GroundChecker
    {

        public Transform Transform { get; private set; }
        public Bounds Bounds { get; private set; }
        public float ExtraHeight { get; private set; }
        public RaycastHit RaycastHit { get; private set; }
        public bool IsObjectGrounded { get; private set; }
    
        public bool IsGrounded(Transform transform, Bounds bounds, float extraHeight, out RaycastHit hit)
        {
            IsObjectGrounded = Physics.BoxCast(bounds.center,
                bounds.extents / 2,
                Vector3.down,
                out hit,
                transform.rotation,
                bounds.extents.y / 2 + extraHeight);
        
            return IsObjectGrounded;
        }
    }
}
