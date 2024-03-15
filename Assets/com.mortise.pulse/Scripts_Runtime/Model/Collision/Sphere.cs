using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    internal class Sphere {

        FVector2 center;
        internal FVector2 Center => center;

        float radius;
        internal float Radius => radius;

        internal Sphere(FVector2 center, float radius) {
            this.center = center;
            this.radius = radius;
        }

        internal bool Contains(FVector2 point) {
            var diff = point - center;
            if (diff.SqrMagnitude() <= radius * radius) {
                return true;
            }
            return false;
        }

    }

}