using System.Collections;
using System.Collections.Generic;
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
            aabb1AABB = new AABB(aabb1.position - aabb1.localScale / 2, aabb1.position + aabb1.localScale / 2);
            aabb2AABB = new AABB(aabb2.position - aabb2.localScale / 2, aabb2.position + aabb2.localScale / 2);
            circleCircle = new Circle(circle.position, circle.localScale.x / 2);
            obbOBB = new OBB(obb.position, obb.localScale, obb.eulerAngles.z * Mathf.Deg2Rad);
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
            Gizmos.DrawWireCube(aabb.Center, aabb.Size);
        }

        void OnDrawCircle(Circle circle, Color color) {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(circle.Center, circle.Radius);
        }

        void OnDrawOBB(OBB obb, Color color) {
            Gizmos.color = color;
            Vector2[] vertices = obb.Vertices;
            for (int i = 0; i < vertices.Length; i++) {
                Gizmos.DrawLine(vertices[i], vertices[(i + 1) % vertices.Length]);
            }
        }

    }

}