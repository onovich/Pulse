using UnityEngine;

namespace MortiseFrame.Pulse.Sample {

    public static class GizmosHeler {

        public static void OnDrawShape(MortiseFrame.Pulse.RigidbodyEntity rb, Color color) {
            if (rb.Shape is BoxShape) {
                OnDrawBox(rb.Shape as BoxShape, rb.Transform, color);
            } else if (rb.Shape is CircleShape) {
                OnDrawCircle(rb.Shape as CircleShape, rb.Transform, color);
            }
        }

        static void OnDrawRay(MortiseFrame.Abacus.Vector2 origin, MortiseFrame.Abacus.Vector2 direction, Color color) {
            Gizmos.color = color;
            Gizmos.DrawRay(new Vector3(origin.x, origin.y, 0), new Vector3(direction.x, direction.y, 0));
        }

        static void OnDrawPoint(MortiseFrame.Abacus.Vector2 point, Color color) {
            Gizmos.color = color;
            Gizmos.DrawSphere(new Vector3(point.x, point.y, 0), 0.1f);
        }

        static void OnDrawBox(BoxShape box, Transform transform, Color color) {
            if (transform.RadAngle == 0) {
                OnDrawAABB(box.GetAABB(transform), color);
            } else {
                OnDrawOBB(box.GetOBB(transform), color);
            }
        }

        static void OnDrawAABB(AABB aabb, Color color) {
            Gizmos.color = color;
            var center = new Vector3(aabb.Center.x, aabb.Center.y, 0);
            var size = new Vector3(aabb.Size.x, aabb.Size.y, 0);
            Gizmos.DrawWireCube(center, size);
        }

        static void OnDrawCircle(CircleShape circle, Transform transform, Color color) {
            var sphere = circle.GetSphere(transform);
            OnDrawSphere(sphere, color);
        }

        static void OnDrawSphere(Sphere sphere, Color color) {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(new Vector3(sphere.Center.x, sphere.Center.y, 0), sphere.Radius);
        }

        static void OnDrawOBB(OBB obb, Color color) {
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