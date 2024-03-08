using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public class PhysicalCore {

        public PhysicalEventCenter EventCenter { get; private set; }
        PhysicalContext context;

        public PhysicalCore() {
            context = new PhysicalContext();
            EventCenter = new PhysicalEventCenter();
        }

        // Gravity
        public void SetGravity(Vector2 gravity) {
            context.SetGravity(gravity);
        }

        // RB
        public void Rigidbody_Add(RigidbodyEntity rb) {
            context.Rigidbody_Add(rb);
        }

        public void Rigidbody_Remove(RigidbodyEntity rb) {
            context.Rigidbody_Remove(rb);
        }

        public int Rigidbody_TakeAll(out RigidbodyEntity[] res) {
            return context.Rigidbody_TakeAll(out res);
        }

        public void Tick(float dt) {

            // 重力 阻力 速度
            ForcePhase.Tick(context, dt);

            // 粗筛
            PrunePhase.Tick(context, dt);

            // 触发 Trigger Exit
            TriggerExitPhase.Tick(context, dt);

            // 交叉检测
            IntersectPhase.Tick(context, dt);

            // 触发 Trigger Enter / Stay
            TriggerEnterPhase.Tick(context, dt);
            TriggerStayPhase.Tick(context, dt);

            // 触发 Collision Exit
            CollisionExitPhase.Tick(context, dt);

            // 穿透处理
            PenetratePhase.Tick(context, dt);

            // 触发 Collision Enter / Stay
            CollisionEnterPhase.Tick(context, dt);
            CollisionStayPhase.Tick(context, dt);

        }

    }

}