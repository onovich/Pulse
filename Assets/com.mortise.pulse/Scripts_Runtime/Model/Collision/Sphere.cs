using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public class Sphere {

        Vector2 center;
        public Vector2 Center => center;

        float radius;
        public float Radius => radius;

        public Sphere(Vector2 center, float radius) {
            this.center = center;
            this.radius = radius;
        }

        public bool Contains(Vector2 point) {
            var diff = point - center;
            if (diff.SqrMagnitude() <= radius * radius) {
                return true;
            }
            return false;
        }

    }

}