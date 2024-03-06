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

        // Trigger
        bool isTrigger;
        public bool IsTrigger => isTrigger;

        // Static
        bool isStatic;
        public bool IsStatic => isStatic;

        // Velocity
        Vector2 velocity;
        public Vector2 Velocity => velocity;

        // Gravity
        float gravityScale;
        public float GravityScale => gravityScale;

        RigidbodyEntity(Vector2 pos, IShape shape) {
            this.id = IDService.PickRigidbodyID();
            this.transform = new Transform(pos);
            this.shape = shape;
            this.velocity = Vector2.zero;
            this.gravityScale = 0;
            this.isTrigger = false;
            this.isStatic = false;
            this.layer = 0;
            this.holderType = 0;
            this.holderID = 0;
        }

        // Layer
        public void SetLayer(int value) => layer = value;

        // Trigger
        public void SetIsTrigger(bool value) => isTrigger = value;

        // Static 
        public void SetIsStatic(bool value) => isStatic = value;

        // Velocity
        public void SetVelocity(Vector2 value) => velocity = value;

        // Gravity
        public void SetGravityScale(float value) => gravityScale = value;

        // Holder
        public void SetHolder(int type, int id) {
            holderType = type;
            holderID = id;
        }

        // Transform
        public void SetPos(Vector2 pos) {
            transform.SetPos(pos);
        }

        public void SetLocalPos(Vector2 localPos) {
            transform.SetLocalPos(localPos);
        }

        public void CalculateWorldPosition(Transform parent) {

            if (transform.LocalPos == Vector2.zero) {
                return;
            }

            transform.SetPos(parent.Pos + transform.LocalPos);
        }

    }

}