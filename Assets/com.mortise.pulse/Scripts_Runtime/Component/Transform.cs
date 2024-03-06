using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public class Transform {

        // World
        Vector2 pos;
        public Vector2 Pos => pos;

        float radAngle;
        public float RadAngle => radAngle;

        // Local
        Vector2 localPos;
        public Vector2 LocalPos => localPos;

        public Transform(Vector2 pos) {
            this.pos = pos;
        }

        public void SetPos(Vector2 pos) {
            this.pos = pos;
        }

        public void SetLocalPos(Vector2 localPos) {
            this.localPos = localPos;
        }

    }

}