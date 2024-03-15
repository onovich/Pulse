using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    internal static class PhysicalFactory {

        internal static RigidbodyEntity CreateBoxRB(FVector2 pos, FVector2 size, float degAngle) {
            var shape = new BoxShape(size);
            var rb = new RigidbodyEntity(pos, shape);
            rb.SetRadAngle(degAngle * FMath.Deg2Rad);
            var id = IDService.PickRigidbodyID();
            rb.SetID(id);
            return rb;
        }

        internal static RigidbodyEntity CreateCircleRB(FVector2 pos, float radius) {
            var shape = new CircleShape(radius);
            var rb = new RigidbodyEntity(pos, shape);
            var id = IDService.PickRigidbodyID();
            rb.SetID(id);
            return rb;
        }

    }

}