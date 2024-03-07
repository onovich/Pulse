using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public class CircleShape : IShape {

        float radius;
        public float Radius => radius;

        public CircleShape(float radius) {
            this.radius = radius;
        }

        public Sphere GetSphere(Transform transform) {
            return new Sphere(transform.Pos, radius);
        }

        public bool Contains(Vector2 point) {
            var diff = point;
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