using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public class Transform {

        // World
        Vector2 pos;
        public Vector2 Pos => pos;

        float radAngle;
        public float RadAngle => radAngle;

        public Transform(Vector2 pos) {
            this.pos = pos;
        }

        public void SetPos(Vector2 pos) {
            this.pos = pos;
        }

    }

}