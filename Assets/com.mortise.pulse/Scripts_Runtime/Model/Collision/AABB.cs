using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public struct AABB {

        Vector2 min;
        public Vector2 Min => min;

        Vector2 max;
        public Vector2 Max => max;

        public AABB(Vector2 center, Vector2 size) {
            min = center - size / 2;
            max = center + size / 2;
        }

        public Vector2 Center => GetCenter();
        public Vector2 Size => GetSize();
        public Vector2[] Axis => GetAxis();

        public bool Contains(Vector2 point) {
            return point.x >= Min.x && point.x <= Max.x && point.y >= Min.y && point.y <= Max.y;
        }

        Vector2 GetCenter() {
            return (Min + Max) / 2;
        }

        Vector2 GetSize() {
            return Max - Min;
        }

        float GetHeight() {
            return Max.y - Min.y;
        }

        float GetWidth() {
            return Max.x - Min.x;
        }

        public void SetCenter(Vector2 center) {
            var size = GetSize();
            min = center - size / 2;
            max = center + size / 2;
        }

        public (float Min, float Max) ProjectOntoAxis(Vector2 axis) {
            Vector2[] vertices = new Vector2[4];
            vertices[0] = this.Min;
            vertices[1] = new Vector2(this.Min.x, this.Max.y);
            vertices[2] = this.Max;
            vertices[3] = new Vector2(this.Max.x, this.Min.y);

            float Min = Vector2.Dot(vertices[0], axis);
            float Max = Min;

            for (int i = 1; i < vertices.Length; i++) {
                float dotProduct = Vector2.Dot(vertices[i], axis);
                if (dotProduct < Min) {
                    Min = dotProduct;
                } else if (dotProduct > Max) {
                    Max = dotProduct;
                }
            }

            return (Min, Max);
        }

        Vector2[] GetAxis() {
            Vector2[] axes = new Vector2[2];
            axes[0] = new Vector2(1, 0);
            axes[1] = new Vector2(0, 1);
            return axes;
        }

    }

}