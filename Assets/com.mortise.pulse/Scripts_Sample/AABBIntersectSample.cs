using UnityEngine;

namespace MortiseFrame.Pulse.Sample {

    public class AABBIntersectSample : MonoBehaviour {

        [SerializeField] Transform aabb1;
        [SerializeField] Transform aabb2;
        [SerializeField] Transform circle;
        [SerializeField] Transform obb;

        [SerializeField] bool epsilonDirection = false;

        private AABB aabb1AABB;
        private AABB aabb2AABB;
        private Circle circleCircle;
        private OBB obbOBB;

        void RefreshBounds() {
            var min1 = new MortiseFrame.Abacus.Vector2(aabb1.position.x - aabb1.localScale.x / 2, aabb1.position.y - aabb1.localScale.y / 2);
            var max1 = new MortiseFrame.Abacus.Vector2(aabb1.position.x + aabb1.localScale.x / 2, aabb1.position.y + aabb1.localScale.y / 2);
            var min2 = new MortiseFrame.Abacus.Vector2(aabb2.position.x - aabb2.localScale.x / 2, aabb2.position.y - aabb2.localScale.y / 2);
            var max2 = new MortiseFrame.Abacus.Vector2(aabb2.position.x + aabb2.localScale.x / 2, aabb2.position.y + aabb2.localScale.y / 2);
            var circleCenter = new MortiseFrame.Abacus.Vector2(circle.position.x, circle.position.y);
            var circleRadius = circle.localScale.x / 2;
            var obbCenter = new MortiseFrame.Abacus.Vector2(obb.position.x, obb.position.y);
            var obbSize = new MortiseFrame.Abacus.Vector2(obb.localScale.x, obb.localScale.y);
            aabb1AABB = new AABB(min1, max1);
            aabb2AABB = new AABB(min2, max2);
            circleCircle = new Circle(circleCenter, circleRadius);
            obbOBB = new OBB(obbCenter, obbSize, obb.eulerAngles.z * Mathf.Deg2Rad);
        }

        void OnDrawGizmos() {

            if (aabb1 == null || aabb2 == null || circle == null || obb == null) {
                return;
            }

            RefreshBounds();
            var epsilon = epsilonDirection ? float.Epsilon : -float.Epsilon;

            OnDrawAABB(aabb1AABB, Color.white);
            OnDrawAABB(aabb2AABB, Color.green);
            OnDrawCircle(circleCircle, Color.green);
            OnDrawOBB(obbOBB, Color.green);

            if (IsIntersectAABB_AABB(aabb1AABB, aabb2AABB, epsilon)) {
                OnDrawAABB(aabb1AABB, Color.red);
                OnDrawAABB(aabb2AABB, Color.red);
            }
            if (IsIntersectAABB_Circle(aabb1AABB, circleCircle, epsilon)) {
                OnDrawAABB(aabb1AABB, Color.red);
                OnDrawCircle(circleCircle, Color.red);
            }
            if (IsIntersectAABB_OBB(aabb1AABB, obbOBB, epsilon)) {
                OnDrawAABB(aabb1AABB, Color.red);
                OnDrawOBB(obbOBB, Color.red);
            }

        }

        bool IsIntersectAABB_AABB(AABB a, AABB b, float epsilon) {
            return InsersectPF.IsIntersectAABB_AABB(a, b, epsilon);
        }

        bool IsIntersectAABB_Circle(AABB aabb, Circle circle, float epsilon) {
            return InsersectPF.IsIntersectAABB_Circle(aabb, circle, epsilon);
        }

        bool IsIntersectAABB_OBB(AABB aabb, OBB obb, float epsilon) {
            return InsersectPF.IsIntersectAABB_OBB(aabb, obb, epsilon);
        }

        void OnDrawAABB(AABB aabb, Color color) {
            Gizmos.color = color;
            var center = new Vector3(aabb.Center.x, aabb.Center.y, 0);
            var size = new Vector3(aabb.Size.x, aabb.Size.y, 0);
            Gizmos.DrawWireCube(center, size);
        }

        void OnDrawCircle(Circle circle, Color color) {
            Gizmos.color = color;
            var circleCenter = new Vector3(circle.Center.x, circle.Center.y, 0);
            Gizmos.DrawWireSphere(circleCenter, circle.Radius);
        }

        void OnDrawOBB(OBB obb, Color color) {
            Gizmos.color = color;
            MortiseFrame.Abacus.Vector2[] vertices = obb.Vertices;
            for (int i = 0; i < vertices.Length; i++) {
                var vertex = new Vector3(vertices[i].x, vertices[i].y, 0);
                var nextVertex = new Vector3(vertices[(i + 1) % vertices.Length].x, vertices[(i + 1) % vertices.Length].y, 0);
                Gizmos.DrawLine(vertex, nextVertex);
            }
        }

    }

}