using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public class BoxShape : IShape {

        Vector2 size;
        public Vector2 Size => size;

        BoxShape(Vector2 size) {
            this.size = size;
        }

        public AABB GetAABB(Transform transform) {
            return new AABB(transform.Pos, size);
        }

        public OBB GetOBB(Transform transform) {
            return new OBB(transform.Pos, size, transform.RadAngle);
        }

        public void SetSize(Vector2 size) {
            this.size = size;
        }

        AABB IShape.GetPruneBounding(Transform transform) {
            if (transform.RadAngle == 0) {
                return new AABB(transform.Pos, size);
            } else {
                float maxSide = Mathf.Max(size.x, size.y);
                Vector2 targetSize = new Vector2(maxSide, maxSide);
                return new AABB(transform.Pos, targetSize);
            }
        }

    }

}