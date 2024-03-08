using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public static class PenetratePhase {

        public static void Tick(PhysicalContext context, float dt) {

            context.CollisionContact_ForEach((kv) => {
                var a = kv.Item2;
                var b = kv.Item3;
                ApplyRestore(context, a, b);
                ApplyBounce(context, a, b);
                ApplyCollisionStay(context, a, b);
            });

        }

        static void ApplyRestore(PhysicalContext context, RigidbodyEntity a, RigidbodyEntity b) {
            if (a.IsStatic && !b.IsStatic) {
                RestoreDynamic_Static(context, b, a);
            }

            if (!a.IsStatic && b.IsStatic) {
                RestoreDynamic_Static(context, a, b);
            }

            if (!a.IsStatic && !b.IsStatic) {
                RestoreDynamic_Dynamic(context, a, b);
            }
        }

        static void ApplyCollisionStay(PhysicalContext context, RigidbodyEntity a, RigidbodyEntity b) {
            if (context.CollisionContact_Contains(a, b)) {
                context.EnqueueCollisionStay(a, b);
            }
        }

        static void ApplyBounce(PhysicalContext context, RigidbodyEntity a, RigidbodyEntity b) {
            if (a.IsStatic && b.IsStatic) {
                return;
            }

            // 计算碰撞法线
            var collisionNormal = (b.Transform.Pos - a.Transform.Pos).normalized;
            var relativeVelocity = b.Velocity - a.Velocity;
            var velocityAlongNormal = FVector2.Dot(relativeVelocity, collisionNormal);

            // 如果速度沿法线方向是远离的，不应用反弹
            if (velocityAlongNormal > 0) {
                return;
            }

            // 计算弹性系数
            var aRestitution = a.Material == null ? 0f : a.Material.Restitution;
            var bRestitution = b.Material == null ? 0f : b.Material.Restitution;

            // 计算反弹速度
            var aBounceVelocity = -velocityAlongNormal * aRestitution;
            var bBounceVelocity = -velocityAlongNormal * bRestitution;

            // 应用反弹速度
            if (!a.IsStatic) {
                a.SetVelocity(a.Velocity + collisionNormal * aBounceVelocity);
            }
            if (!b.IsStatic) {
                b.SetVelocity(b.Velocity - collisionNormal * bBounceVelocity);
            }
        }

        static void RestoreDynamic_Static(PhysicalContext context, RigidbodyEntity d, RigidbodyEntity s) {

            var overlapDepth = PenetratePF.PenetrateDepthRB_RB(d, s);
            if (overlapDepth == FVector2.zero) {
                return;
            }
            d.Transform.SetPos(d.Transform.Pos + overlapDepth);

        }

        static void RestoreDynamic_Dynamic(PhysicalContext context, RigidbodyEntity a, RigidbodyEntity b) {

            var overlapDepth = PenetratePF.PenetrateDepthRB_RB(a, b);
            if (overlapDepth == FVector2.zero) {
                return;
            }
            a.Transform.SetPos(a.Transform.Pos + overlapDepth);
            b.Transform.SetPos(b.Transform.Pos - overlapDepth);

        }

    }

}