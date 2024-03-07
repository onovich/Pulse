using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public static class PenetratePhase {

        public static void Tick(PhysicalContext context, float dt) {

            context.CollisionContact_ForEach((kv) => {
                var a = kv.Item2;
                var b = kv.Item3;
                if (a.IsStatic && b.IsStatic) {
                    return;
                }
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

        }

        static void RestoreDynamic_Static(PhysicalContext context, RigidbodyEntity d, RigidbodyEntity s) {

            var overlapDepth = PenetratePF.PenetrateDepthRB_RB(d, s);
            if (overlapDepth == Vector2.zero) {
                return;
            }
            d.Transform.SetPos(d.Transform.Pos + overlapDepth);

        }

        static void RestoreDynamic_Dynamic(PhysicalContext context, RigidbodyEntity a, RigidbodyEntity b) {

            var overlapDepth = PenetratePF.PenetrateDepthRB_RB(a, b);
            if (overlapDepth == Vector2.zero) {
                return;
            }
            a.Transform.SetPos(a.Transform.Pos + overlapDepth);
            throw new System.Exception("未实现 Dynamic-Dynamic 碰撞恢复");

        }

    }

}