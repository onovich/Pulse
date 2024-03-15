using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    internal class CircleShape : IShape {

        float radius;
        internal float Radius => radius;

        internal CircleShape(float radius) {
            this.radius = radius;
        }

        internal Sphere GetSphere(TFComponent transform) {
            return new Sphere(transform.Pos, radius);
        }

        internal bool Contains(FVector2 point) {
            var diff = point;
            if (diff.SqrMagnitude() <= radius * radius) {
                return true;
            }
            return false;
        }

        AABB IShape.GetPruneBounding(TFComponent transform) {
            return new AABB(transform.Pos, new FVector2(radius * 2, radius * 2));
        }

    }

}