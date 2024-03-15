using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    internal class TFComponent {

        // World
        FVector2 pos;
        internal FVector2 Pos => pos;

        float radAngle;
        internal float RadAngle => radAngle;

        internal TFComponent(FVector2 pos) {
            this.pos = pos;
        }

        internal void SetPos(FVector2 pos) {
            this.pos = pos;
        }

        internal void SetRadAngle(float radAngle) {
            this.radAngle = radAngle;
        }

    }

}