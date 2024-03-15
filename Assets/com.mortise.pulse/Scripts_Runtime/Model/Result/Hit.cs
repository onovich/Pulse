using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public struct Hits {

        public FVector2 point;
        public FVector2 normal;

        public Hits(FVector2 point, FVector2 normal) {
            this.point = point;
            this.normal = normal;
        }

    }

}