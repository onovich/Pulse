using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    internal class BoxShape : IShape {

        FVector2 size;
        internal FVector2 Size => size;

        internal BoxShape(FVector2 size) {
            this.size = size;
        }

        internal AABB GetAABB(TFComponent transform) {
            return new AABB(transform.Pos, size);
        }

        internal OBB GetOBB(TFComponent transform) {
            return new OBB(transform.Pos, size, transform.RadAngle);
        }

        internal void SetSize(FVector2 size) {
            this.size = size;
        }

        AABB IShape.GetPruneBounding(TFComponent transform) {
            if (transform.RadAngle == 0) {
                return new AABB(transform.Pos, size);
            } else {
                float maxSide = FMath.Max(size.x, size.y);
                FVector2 targetSize = new FVector2(maxSide, maxSide);
                return new AABB(transform.Pos, targetSize);
            }
        }

    }

}