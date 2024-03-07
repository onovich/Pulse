using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public class RigidbodyEntity {

        // Base Info
        uint id;
        public uint ID => id;

        // Holder
        int holderType;
        public int HolderType => holderType;

        int holderID;
        public int HolderID => holderID;

        // Layer
        int layer;
        public int Layer => layer;

        // Transform
        Transform transform;
        public Transform Transform => transform;

        // Shape
        IShape shape;
        public IShape Shape => shape;

        // Material
        PhysicalMaterial material;
        public PhysicalMaterial Material => material;

        // Trigger
        bool isTrigger;
        public bool IsTrigger => isTrigger;

        // Static
        bool isStatic;
        public bool IsStatic => isStatic;

        // Velocity
        Vector2 velocity;
        public Vector2 Velocity => velocity;

        // Mass
        float mass;
        public float Mass => mass;

        RigidbodyEntity(Vector2 pos, IShape shape) {
            this.id = IDService.PickRigidbodyID();
            this.transform = new Transform(pos);
            this.shape = shape;
            this.velocity = Vector2.zero;
            this.mass = 0;
            this.isTrigger = false;
            this.isStatic = false;
            this.layer = 0;
            this.holderType = 0;
            this.holderID = 0;
        }

        // Trigger
        public void SetIsTrigger(bool value) => isTrigger = value;

        // Static 
        public void SetIsStatic(bool value) => isStatic = value;

        // Velocity
        public void SetVelocity(Vector2 value) => velocity = value;

        // Gravity
        public void SetMass(float value) => mass = value;

        // Holder
        public void SetHolder(int type, int id) {
            holderType = type;
            holderID = id;
        }

        // Material
        public void SetMaterial(PhysicalMaterial value) => material = value;

        // Transform
        public void SetPos(Vector2 pos) {
            transform.SetPos(pos);
        }

    }

}