using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public static class PenetratePF {

        public static Vector2 PenetrateDepthRB_RB(RigidbodyEntity a, RigidbodyEntity b) {
            IShape aShape = a.Shape;
            IShape bShape = b.Shape;

            // Circle & Circle
            CircleShape aCircle = aShape as CircleShape;
            CircleShape bCircle = bShape as CircleShape;
            if (aCircle != null && bCircle != null) {
                return PenetrateDepthCircle_Circle(a.Transform, aCircle, b.Transform, bCircle);
            }

            // Box & Box
            BoxShape aBox = aShape as BoxShape;
            BoxShape bBox = bShape as BoxShape;
            if (aBox != null && bBox != null) {
                return PenetrateDepthBox_Box(a.Transform, aBox, b.Transform, bBox);
            }

            // Circle & Box
            if (aBox != null && bCircle != null) {
                return PenetrateDepthCircle_Box(b.Transform, bCircle, a.Transform, aBox);
            }
            if (aCircle != null && bBox != null) {
                return PenetrateDepthCircle_Box(a.Transform, aCircle, b.Transform, bBox);
            }

            return Vector2.zero;
        }

        static Vector2 PenetrateDepthBox_Box(Transform aTF, BoxShape aBox, Transform bTF, BoxShape bBox) {
            // AABB & AABB
            if (aTF.RadAngle == 0 && bTF.RadAngle == 0) {
                return PenetrateDepthAABB_AABB(aBox.GetAABB(aTF), bBox.GetAABB(bTF));
            }
            // OBB & OBB
            return PenetrateDepthOBB_OBB(aBox.GetOBB(aTF), bBox.GetOBB(bTF));
        }

        static Vector2 PenetrateDepthCircle_Circle(Transform aTF, CircleShape a, Transform bTF, CircleShape b) {
            Vector2 diff = aTF.Pos - bTF.Pos;
            float distance = diff.magnitude;
            float penetrationDepth = a.Radius + b.Radius - distance;

            if (penetrationDepth > 0) {
                return diff.normalized * penetrationDepth;
            } else {
                return Vector2.zero;
            }
        }

        static Vector2 PenetrateDepthCircle_Box(Transform circleTF, CircleShape circle, Transform boxTF, BoxShape box) {
            // Circle & AABB
            if (boxTF.RadAngle == 0) {
                return PenetrateDepthAABB_Sphere(box.GetAABB(boxTF), circle.GetSphere(circleTF));
            }
            // Circle & OBB
            return PenetrateDepthOBB_Sphere(box.GetOBB(boxTF), circle.GetSphere(circleTF));
        }

        static Vector2 PenetrateDepthAABB_AABB(AABB a, AABB b) {
            float overlapX = Mathf.Min(a.Max.x - b.Min.x, b.Max.x - a.Min.x);
            float overlapY = Mathf.Min(a.Max.y - b.Min.y, b.Max.y - a.Min.y);

            if (overlapX <= 0f || overlapY <= 0f) {
                return Vector2.zero;
            }

            float directionX = ((a.Min.x + a.Max.x) / 2) < ((b.Min.x + b.Max.x) / 2) ? -1 : 1;
            float directionY = ((a.Min.y + a.Max.y) / 2) < ((b.Min.y + b.Max.y) / 2) ? -1 : 1;

            if (overlapX < overlapY) {
                return new Vector2(overlapX * directionX, 0);
            } else {
                return new Vector2(0, overlapY * directionY);
            }
        }

        static Vector2 PenetrateDepthOBB_OBB(OBB obb1, OBB obb2) {
            throw new System.Exception($"未实现 OBB x OBB 的碰撞深度计算");
        }

        static Vector2 PenetrateDepthOBB_Sphere(OBB obb, Sphere sphere) {
            throw new System.Exception($"未实现 Circle x OBB 的碰撞深度计算");
        }

        static Vector2 PenetrateDepthAABB_Sphere(AABB aabb, Sphere sphere) {
            Vector2 closestPoint = new Vector2(
                Mathf.Max(aabb.Min.x, Mathf.Min(sphere.Center.x, aabb.Max.x)),
                Mathf.Max(aabb.Min.y, Mathf.Min(sphere.Center.y, aabb.Max.y))
            );

            Vector2 directionToSphere = sphere.Center - closestPoint;
            float distanceToSphere = directionToSphere.magnitude;
            float penetrationDepth = sphere.Radius - distanceToSphere;

            if (penetrationDepth > 0) {
                return directionToSphere.normalized * penetrationDepth;
            } else {
                return Vector2.zero;
            }
        }

    }

}