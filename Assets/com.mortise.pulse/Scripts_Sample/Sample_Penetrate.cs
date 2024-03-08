using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Pulse.Sample {

    public class Sample_Penetrate : MonoBehaviour {

        [SerializeField] List<UnityEngine.Transform> staticBoxTFs;
        [SerializeField] List<UnityEngine.Transform> dynamicCircleTFs;
        [SerializeField] List<UnityEngine.Transform> dynamicBoxTFs;

        [SerializeField] bool epsilonDirection = false;
        [SerializeField] bool drawPenetrate = true;

        Dictionary<MortiseFrame.Pulse.RigidbodyEntity, UnityEngine.Transform> rbs;
        Dictionary<ulong, (MortiseFrame.Pulse.RigidbodyEntity, MortiseFrame.Pulse.RigidbodyEntity, MortiseFrame.Abacus.Vector2)> contactDicts;

        public uint idRecord = 0;

        [ContextMenu("RefreshShapes")]
        void RefreshShapes() {
            if (staticBoxTFs == null && dynamicCircleTFs == null && dynamicBoxTFs == null) return;

            rbs = new Dictionary<MortiseFrame.Pulse.RigidbodyEntity, UnityEngine.Transform>();
            contactDicts = new Dictionary<ulong, (MortiseFrame.Pulse.RigidbodyEntity, MortiseFrame.Pulse.RigidbodyEntity, MortiseFrame.Abacus.Vector2)>();
            idRecord = 0;

            foreach (var boxTF in staticBoxTFs) {
                if (boxTF == null) continue;
                var shape = new BoxShape(new MortiseFrame.Abacus.Vector2(boxTF.localScale.x, boxTF.localScale.y));
                var pos = new MortiseFrame.Abacus.Vector2(boxTF.position.x, boxTF.position.y);
                var radAngle = boxTF.eulerAngles.z * Mathf.Deg2Rad;
                var rb = new MortiseFrame.Pulse.RigidbodyEntity(pos, shape);
                rb.SetRadAngle(radAngle);
                rb.SetID(++idRecord);
                rb.SetIsStatic(true);
                var boxType = rb.Transform.RadAngle == 0 ? "AABB" : "OBB";
                boxTF.gameObject.name = $"Static_{boxType}_{rb.ID}";
                rbs.Add(rb, boxTF);
            }

            foreach (var circleTF in dynamicCircleTFs) {
                if (circleTF == null) continue;
                var shape = new CircleShape(circleTF.localScale.x / 2);
                var pos = new MortiseFrame.Abacus.Vector2(circleTF.position.x, circleTF.position.y);
                var rb = new MortiseFrame.Pulse.RigidbodyEntity(pos, shape);
                rb.SetID(++idRecord);
                rb.SetIsStatic(false);
                circleTF.gameObject.name = $"Dynamic_Circle_{rb.ID}";
                rbs.Add(rb, circleTF);
            }

            foreach (var boxTF in dynamicBoxTFs) {
                if (boxTF == null) continue;
                var shape = new BoxShape(new MortiseFrame.Abacus.Vector2(boxTF.localScale.x, boxTF.localScale.y));
                var pos = new MortiseFrame.Abacus.Vector2(boxTF.position.x, boxTF.position.y);
                var radAngle = boxTF.eulerAngles.z * Mathf.Deg2Rad;
                var rb = new MortiseFrame.Pulse.RigidbodyEntity(pos, shape);
                rb.SetRadAngle(radAngle);
                rb.SetID(++idRecord);
                rb.SetIsStatic(false);
                var boxType = rb.Transform.RadAngle == 0 ? "Dynamic_AABB" : "OBB";
                boxTF.gameObject.name = $"Dynamic_{boxType}_{rb.ID}";
                rbs.Add(rb, boxTF);
            }
        }

        void OnDrawPenetrate(float epsilon) {
            if (!drawPenetrate) {
                return;
            }

            foreach (var a in rbs.Keys) {
                if (a == null) continue;
                foreach (var b in rbs.Keys) {
                    if (b == null) continue;
                    if (a == b) {
                        continue;
                    }
                    if (a.IsStatic && b.IsStatic) {
                        continue;
                    }
                    if (a.IsStatic) {
                        continue;
                    }
                    var overlapDepth = MortiseFrame.Pulse.PenetratePF.PenetrateDepthRB_RB(a, b);
                    var key = IDService.ContactKey(a.ID, b.ID);
                    if (overlapDepth != MortiseFrame.Abacus.Vector2.zero) {
                        if (!contactDicts.ContainsKey(key)) {
                            contactDicts.Add(key, (a, b, overlapDepth));
                        } else {
                            contactDicts[key] = (a, b, overlapDepth);
                        }
                    } else {
                        if (contactDicts.ContainsKey(key)) {
                            contactDicts.Remove(key);
                        }
                    }
                }
            }

            foreach (var kv in contactDicts.Values) {
                var a = kv.Item1;
                var b = kv.Item2;
                var overlapDepth = kv.Item3;
                var colorA = a.IsStatic ? Color.blue : Color.red;
                var colorB = b.IsStatic ? Color.blue : Color.red;
                GizmosHeler.OnDrawShape(a, colorA);
                GizmosHeler.OnDrawShape(b, colorB);
                if (a.IsStatic && !b.IsStatic) {
                    GizmosHeler.OnDrawLine(b.Transform.Pos, overlapDepth, Color.red);
                } else if (!a.IsStatic && b.IsStatic) {
                    GizmosHeler.OnDrawLine(a.Transform.Pos, overlapDepth, Color.red);
                } else {
                    GizmosHeler.OnDrawLine(a.Transform.Pos, overlapDepth, Color.red);
                }
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
                rb.SetPos(new MortiseFrame.Abacus.Vector2(tf.position.x, tf.position.y));
                rb.SetRadAngle(tf.eulerAngles.z * Mathf.Deg2Rad);
            }

            foreach (var rb in rbs.Keys) {
                var color = rb.IsStatic ? Color.green : Color.white;
                GizmosHeler.OnDrawShape(rb, color);
            }

            OnDrawPenetrate(epsilon);
        }

    }

}