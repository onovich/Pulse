using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public static class IntersectPF {

        public static bool IsIntersectRB_RB(RigidbodyEntity a, RigidbodyEntity b, float epsilon) {
            IShape aShape = a.Shape;
            IShape bShape = b.Shape;

            // Circle & Circle
            CircleShape aCircle = aShape as CircleShape;
            CircleShape bCircleB = bShape as CircleShape;
            if (aCircle != null && bCircleB != null) {
                return IsIntersectCircle_Circle(a.Transform, aCircle, b.Transform, bCircleB, epsilon);
            }

            // Box & Box
            BoxShape aBox = aShape as BoxShape;
            BoxShape bBox = bShape as BoxShape;
            if (aBox != null && bBox != null) {
                return IsIntersectBox_Box(a.Transform, aBox, b.Transform, bBox, epsilon);
            }

            // Circle & Box
            if (aBox != null && bCircleB != null) {
                return IsIntersectCircle_Box(b.Transform, bCircleB, a.Transform, aBox, epsilon);
            }
            if (aCircle != null && bBox != null) {
                return IsIntersectCircle_Box(b.Transform, aCircle, b.Transform, bBox, epsilon);
            }

            return false;
        }

        static bool IsIntersectBox_Box(Transform aTF, BoxShape aBox, Transform bTF, BoxShape bBox, float epsilon) {
            // AABB & AABB
            if (aTF.RadAngle == 0 && bTF.RadAngle == 0) {
                return IsIntersectAABB_AABB(aBox.GetAABB(aTF), bBox.GetAABB(aTF), epsilon);
            }
            // OBB & OBB
            return IsIntersectOBB_OBB(aBox.GetOBB(aTF), bBox.GetOBB(bTF), epsilon);
        }

        static bool IsIntersectCircle_Circle(Transform aTF, CircleShape a, Transform bTF, CircleShape b, float epsilon) {
            var diff = aTF.Pos - bTF.Pos;
            var distanceSquared = diff.sqrMagnitude;
            var radiusSum = a.Radius + b.Radius;
            return radiusSum * radiusSum - distanceSquared > epsilon;
        }

        static bool IsIntersectCircle_Box(Transform circleTF, CircleShape circle, Transform boxTF, BoxShape box, float epsilon) {
            // Circle & AABB
            if (boxTF.RadAngle == 0) {
                return IsIntersectAABB_Circle(box.GetAABB(boxTF), new Sphere(circleTF.Pos, circle.Radius), epsilon);
            }
            // Circle & OBB
            return IsIntersectOBB_Circle(new Sphere(circleTF.Pos, circle.Radius), box.GetOBB(boxTF), epsilon);
        }

        static bool IsIntersectAABB_AABB(AABB a, AABB b, float epsilon) {
            return (b.Max.y - a.Min.y > epsilon) && (a.Max.y - b.Min.y > epsilon) &&
                   (b.Max.x - a.Min.x > epsilon) && (a.Max.x - b.Min.x > epsilon);
        }

        static bool IsIntersectAABB_Circle(AABB aabb, Sphere circle, float epsilon) {
            Vector2 clampedCenter = Vector2.Max(aabb.Min, Vector2.Min(aabb.Max, circle.Center));
            float distanceSquared = (clampedCenter - circle.Center).sqrMagnitude;
            return distanceSquared <= (circle.Radius * circle.Radius + epsilon);
        }

        static bool IsIntersectOBB_OBB(OBB obb1, OBB obb2, float epsilon) {
            Vector2[] axesToTest = { obb1.AxisX, obb1.AxisY, obb2.AxisX, obb2.AxisY };

            foreach (var axis in axesToTest) {
                var projection1 = obb1.ProjectOntoAxis(axis);
                var projection2 = obb2.ProjectOntoAxis(axis);

                if (!ProjectionsOverlap(projection1, projection2, epsilon)) {
                    return false;
                }
            }

            return true;
        }

        static bool IsIntersectOBB_Circle(Sphere circle, OBB obb, float epsilon) {
            Vector2 localSphereCenter = circle.Center - obb.Center;
            Vector2 closestPointOnOBB = obb.Center;

            foreach (var axis in new Vector2[] { obb.AxisX, obb.AxisY }) {
                float distanceOnAxis = Vector2.Dot(localSphereCenter, axis);
                float halfSizeOnAxis = axis == obb.AxisX ? obb.Size.x * 0.5f : obb.Size.y * 0.5f;

                float clampedDistance = Mathf.Clamp(distanceOnAxis, -halfSizeOnAxis, halfSizeOnAxis);
                closestPointOnOBB += axis * clampedDistance;
            }

            return Vector2.Distance(circle.Center, closestPointOnOBB) <= circle.Radius + epsilon;
        }

        static bool ProjectionsOverlap((float Min, float Max) a, (float Min, float Max) b, float epsilon) {

            var res = b.Max - a.Min > epsilon && a.Max - b.Min > epsilon;
            return res;

        }

    }

}