namespace MortiseFrame.Pulse {

    internal static class IDService {

        internal static uint rigidbodyRecordID = 0;

        internal static uint PickRigidbodyID() {
            return rigidbodyRecordID++;
        }

        internal static ulong ContactKey(uint idA, uint idB) {
            if (idA > idB) {
                return (ulong)idA << 32 | (ulong)idB;
            }
            return (ulong)idB << 32 | (ulong)idA;
        }

    }

}