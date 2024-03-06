using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Pulse.Sample {

    public class AABBIntersectSample : MonoBehaviour {
        [SerializeField] List<Transform> aabbTs;
        [SerializeField] List<Transform> circleTs;
        [SerializeField] List<Transform> obbTs;

        [SerializeField] bool epsilonDirection = false;
        [SerializeField] bool drawIntersect = true;
        [SerializeField] bool drawOverlap = false;

        List<AABB> aabbs;
        List<Circle> circles;
        List<OBB> obbs;

        void RefreshBounds() {

            if (aabbTs == null || circleTs == null || obbTs == null) return;

            aabbs = new List<AABB>();
            circles = new List<Circle>();
            obbs = new List<OBB>();

            foreach (var aabbT in aabbTs) {
                if (aabbT == null) continue;
                var aabb = new AABB(new MortiseFrame.Abacus.Vector2(aabbT.position.x - aabbT.localScale.x / 2, aabbT.position.y - aabbT.localScale.y / 2),
                    new MortiseFrame.Abacus.Vector2(aabbT.position.x + aabbT.localScale.x / 2, aabbT.position.y + aabbT.localScale.y / 2));
                aabbs.Add(aabb);
            }

            foreach (var circleT in circleTs) {
                if (circleT == null) continue;
                var circle = new Circle(new MortiseFrame.Abacus.Vector2(circleT.position.x, circleT.position.y), circleT.localScale.x / 2);
                circles.Add(circle);
            }

            foreach (var obbT in obbTs) {
                if (obbT == null) continue;
                var obb = new OBB(new MortiseFrame.Abacus.Vector2(obbT.position.x, obbT.position.y),
                    new MortiseFrame.Abacus.Vector2(obbT.localScale.x, obbT.localScale.y), obbT.eulerAngles.z * Mathf.Deg2Rad);
                obbs.Add(obb);
            }

        }

        void OnDrawIntersect(float epsilon) {
            if (!drawIntersect) {
                return;
            }

            for (int i = 0; i < aabbs.Count; i++) {
                for (int j = 0; j < aabbs.Count; j++) {
                    if (i == j) {
                        continue;
                    }
                    if (MortiseFrame.Pulse.InsersectPF.IsIntersectAABB_AABB(aabbs[i], aabbs[j], epsilon)) {
                        OnDrawAABB(aabbs[i], Color.red);
                        OnDrawAABB(aabbs[j], Color.red);
                    }
                }
            }

            for (int i = 0; i < aabbs.Count; i++) {
                for (int j = 0; j < circles.Count; j++) {
                    if (MortiseFrame.Pulse.InsersectPF.IsIntersectAABB_Circle(aabbs[i], circles[j], epsilon)) {
                        OnDrawAABB(aabbs[i], Color.red);
                        OnDrawCircle(circles[j], Color.red);
                    }
                }
            }

            for (int i = 0; i < aabbs.Count; i++) {
                for (int j = 0; j < obbs.Count; j++) {
                    if (MortiseFrame.Pulse.InsersectPF.IsIntersectAABB_OBB(aabbs[i], obbs[j], epsilon)) {
                        OnDrawAABB(aabbs[i], Color.red);
                        OnDrawOBB(obbs[j], Color.red);
                    }
                }
            }

            for (int i = 0; i < circles.Count; i++) {
                for (int j = 0; j < circles.Count; j++) {
                    if (i == j) {
                        continue;
                    }
                    if (MortiseFrame.Pulse.InsersectPF.IsIntersectCircle_Circle(circles[i], circles[j], epsilon)) {
                        OnDrawCircle(circles[i], Color.red);
                        OnDrawCircle(circles[j], Color.red);
                    }
                }
            }

            for (int i = 0; i < obbs.Count; i++) {
                for (int j = 0; j < obbs.Count; j++) {
                    if (i == j) {
                        continue;
                    }
                    if (MortiseFrame.Pulse.InsersectPF.IsIntersectOBB_OBB(obbs[i], obbs[j], epsilon)) {
                        OnDrawOBB(obbs[i], Color.red);
                        OnDrawOBB(obbs[j], Color.red);
                    }
                }
            }

            for (int i = 0; i < circles.Count; i++) {
                for (int j = 0; j < obbs.Count; j++) {
                    if (MortiseFrame.Pulse.InsersectPF.IsIntersectCircle_OBB(circles[i], obbs[j], epsilon)) {
                        OnDrawCircle(circles[i], Color.red);
                        OnDrawOBB(obbs[j], Color.red);
                    }
                }
            }
        }

        void OnDrawOverlap(float epsilon) {
            if (!drawOverlap) {
                return;
            }

            for (int i = 0; i < aabbs.Count; i++) {
                for (int j = 0; j < circles.Count; j++) {
                    if (MortiseFrame.Pulse.InsersectPF.OverlapCircle_AABB(circles[j], aabbs[i], MortiseFrame.Abacus.Vector2.zero, epsilon, out var hits)) {
                        OnDrawAABB(aabbs[i], Color.red);
                        OnDrawCircle(circles[j], Color.red);
                        var hitPoint = hits.point;
                        var hitNormal = hits.normal;
                        OnDrawPoint(hitPoint, Color.red);
                        OnDrawRay(hitPoint, hitNormal, Color.red);
                    }
                }
            }
        }

        void OnDrawGizmos() {
            var epsilon = epsilonDirection ? float.Epsilon : -float.Epsilon;

            RefreshBounds();

            foreach (var aabb in aabbs) {
                OnDrawAABB(aabb, Color.white);
            }

            foreach (var circle in circles) {
                OnDrawCircle(circle, Color.white);
            }

            foreach (var obb in obbs) {
                OnDrawOBB(obb, Color.white);
            }

            OnDrawIntersect(epsilon);
            OnDrawOverlap(epsilon);

        }

        void OnDrawRay(MortiseFrame.Abacus.Vector2 origin, MortiseFrame.Abacus.Vector2 direction, Color color) {
            Gizmos.color = color;
            Gizmos.DrawRay(new Vector3(origin.x, origin.y, 0), new Vector3(direction.x, direction.y, 0));
        }

        void OnDrawPoint(MortiseFrame.Abacus.Vector2 point, Color color) {
            Gizmos.color = color;
            Gizmos.DrawSphere(new Vector3(point.x, point.y, 0), 0.1f);
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