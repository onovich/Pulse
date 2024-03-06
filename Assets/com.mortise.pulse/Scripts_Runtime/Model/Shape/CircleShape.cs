using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public class CircleShape : IShape {

        Vector2 center;
        public Vector2 Center => center;

        float radius;
        public float Radius => radius;

        public CircleShape(float radius) {
            this.radius = radius;
        }

        public bool Contains(Vector2 point) {
            var diff = point - center;
            if (diff.SqrMagnitude() <= radius * radius) {
                return true;
            }
            return false;
        }

        AABB IShape.GetPruneBounding(Transform transform) {
            return new AABB(transform.Pos, new Vector2(radius * 2, radius * 2));
        }

    }

}