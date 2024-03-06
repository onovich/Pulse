namespace MortiseFrame.Pulse {

    public static class PenetratePhase {

        public static void Tick(PhysicalContext context, float dt) {

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

        static void RestoreDynamic_Static(PhysicalContext context, RigidbodyEntity d, RigidbodyEntity s) {

            var dShape = d.Shape;
            var sShape = s.Shape;

            var dTF = d.Transform;
            var sTF = s.Transform;

            var dBox = dShape as BoxShape;
            var sBox = sShape as BoxShape;

            var dCircle = dShape as CircleShape;
            var sCircle = sShape as CircleShape;

            if (dBox != null && sBox != null) {
                if (dTF.RadAngle == 0 && sTF.RadAngle == 0) {
                    RestoreDynamic_Static_AABB_AABB(context, d, dBox, s, sBox);
                } else {
                    RestoreDynamic_Static_OBB_OBB(context, d, dBox, s, sBox);
                }
                return;
            }

            if (dCircle != null && sCircle != null) {
                RestoreDynamic_Static_Circle_Circle(context, d, dCircle, s, sCircle);
                return;
            }

            if (dBox != null && sCircle != null) {
                RestoreDynamic_Static_AABB_Circle(context, d, dBox, s, sCircle);
                return;
            }

            if (dCircle != null && sBox != null) {
                RestoreDynamic_Static_AABB_Circle(context, s, sBox, d, dCircle);
                return;
            }

        }

        static void RestoreDynamic_Dynamic(PhysicalContext context, RigidbodyEntity a, RigidbodyEntity b) {

        }

        static void RestoreDynamic_Static_AABB_AABB(PhysicalContext context, RigidbodyEntity d, BoxShape dBox, RigidbodyEntity s, BoxShape sBox) {


        }

        static void RestoreDynamic_Static_OBB_OBB(PhysicalContext context, RigidbodyEntity d, BoxShape dBox, RigidbodyEntity s, BoxShape sBox) {

        }

        static void RestoreDynamic_Static_Circle_Circle(PhysicalContext context, RigidbodyEntity d, CircleShape dCircle, RigidbodyEntity s, CircleShape sCircle) {

        }

        static void RestoreDynamic_Static_AABB_Circle(PhysicalContext context, RigidbodyEntity d, BoxShape dBox, RigidbodyEntity s, CircleShape sCircle) {

        }

    }

}