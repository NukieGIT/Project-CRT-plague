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

        public bool IsGrounded(Transform transform, Bounds bounds, float extraHeight, out RaycastHit[] hit, int layerMask)
        {
            var hitsArray = new RaycastHit[5];
            var hits = Physics.BoxCastNonAlloc(bounds.center,
                bounds.extents / 2,
                Vector3.down,
                hitsArray,
                transform.rotation,
                bounds.extents.y / 2 + extraHeight,
                layerMask);

            hit = hitsArray;
            IsObjectGrounded = hits > 0;
            
            return IsObjectGrounded;
        }
    }
}
