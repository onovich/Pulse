using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Pulse.Sample {

    public class Sample_Intersect : MonoBehaviour {

        [SerializeField] List<UnityEngine.Transform> boxTFs;
        [SerializeField] List<UnityEngine.Transform> circleTFs;

        [SerializeField] bool epsilonDirection = false;
        [SerializeField] bool drawIntersect = true;

        Dictionary<MortiseFrame.Pulse.RigidbodyEntity, UnityEngine.Transform> rbs;
        Dictionary<ulong, (MortiseFrame.Pulse.RigidbodyEntity, MortiseFrame.Pulse.RigidbodyEntity)> contactDicts;

        public uint idRecord = 0;

        [ContextMenu("RefreshShapes")]
        void RefreshShapes() {
            if (boxTFs == null && circleTFs == null) return;

            rbs = new Dictionary<MortiseFrame.Pulse.RigidbodyEntity, UnityEngine.Transform>();
            contactDicts = new Dictionary<ulong, (MortiseFrame.Pulse.RigidbodyEntity, MortiseFrame.Pulse.RigidbodyEntity)>();
            idRecord = 0;

            foreach (var boxTF in boxTFs) {
                if (boxTF == null) continue;
                var shape = new BoxShape(new MortiseFrame.Abacus.FVector2(boxTF.localScale.x, boxTF.localScale.y));
                var pos = new MortiseFrame.Abacus.FVector2(boxTF.position.x, boxTF.position.y);
                var radAngle = boxTF.eulerAngles.z * Mathf.Deg2Rad;
                var rb = new MortiseFrame.Pulse.RigidbodyEntity(pos, shape);
                rb.SetRadAngle(radAngle);
                rb.SetID(++idRecord);
                var boxType = rb.Transform.RadAngle == 0 ? "AABB" : "OBB";
                boxTF.gameObject.name = $"{boxType}_{rb.ID}";
                rbs.Add(rb, boxTF);
            }

            foreach (var circleTF in circleTFs) {
                if (circleTF == null) continue;
                var shape = new CircleShape(circleTF.localScale.x / 2);
                var pos = new MortiseFrame.Abacus.FVector2(circleTF.position.x, circleTF.position.y);
                var rb = new MortiseFrame.Pulse.RigidbodyEntity(pos, shape);
                rb.SetID(++idRecord);
                circleTF.gameObject.name = $"Circle_{rb.ID}";
                rbs.Add(rb, circleTF);
            }
        }

        void OnDrawIntersect(float epsilon) {
            if (!drawIntersect) {
                return;
            }

            foreach (var a in rbs.Keys) {
                if (a == null) continue;
                foreach (var b in rbs.Keys) {
                    if (b == null) continue;
                    if (a == b) {
                        continue;
                    }
                    var isIntersect = MortiseFrame.Pulse.IntersectPF.IsIntersectRB_RB(a, b, epsilon);
                    var key = IDService.ContactKey(a.ID, b.ID);
                    if (isIntersect) {
                        if (!contactDicts.ContainsKey(key)) {
                            contactDicts.Add(key, (a, b));
                        }
                    } else {
                        if (contactDicts.ContainsKey(key)) {
                            contactDicts.Remove(key);
                        }
                    }
                }
            }

            foreach (var v in contactDicts.Values) {
                var a = v.Item1;
                var b = v.Item2;
                var color = Color.red;
                GizmosHeler.OnDrawRBShape(a, color);
                GizmosHeler.OnDrawRBShape(b, color);
            }
        }

        void OnDrawGizmos() {
            if (rbs == null || rbs.Count == 0) {
                return;
            }
            var epsilon = epsilonDirection ? float.Epsilon : -float.Epsilon;

            foreach (var kv in rbs) {
                if (kv.Value == null) continue;
                var rb = kv.Key;
                var tf = kv.Value;
                rb.SetPos(new MortiseFrame.Abacus.FVector2(tf.position.x, tf.position.y));
                rb.SetRadAngle(tf.eulerAngles.z * Mathf.Deg2Rad);
            }

            foreach (var rb in rbs.Keys) {
                GizmosHeler.OnDrawRBShape(rb, Color.white);
            }

            OnDrawIntersect(epsilon);
        }

    }

}