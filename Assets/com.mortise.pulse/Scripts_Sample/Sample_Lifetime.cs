using System.Collections.Generic;
using UnityEngine;
using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse.Sample {

    public class Sample_Lifetime : MonoBehaviour {

        [SerializeField] Transform[] staticBoxTFs;
        [SerializeField] Transform[] dynamicBoxTFs;
        [SerializeField] Transform[] dynamicCircleTFs;

        [SerializeField] Vector2[] dynamicBoxVels;
        [SerializeField] Vector2[] dynamicCircleVels;

        [SerializeField] Sprite staticBoxSprite;
        [SerializeField] Sprite dynamicBoxSprite;
        [SerializeField] Sprite dynamicCircleSprite;

        [SerializeField] float gravity;

        PhysicalCore core;
        uint[] staticBoxIDs;
        uint[] dynamicBoxIDs;
        uint[] dynamicCircleIDs;

        void Start() {

            // 创建物理核心
            core = new PhysicalCore();

            // 设置重力
            core.SetGravity(new FVector2(0, gravity));

            // 初始化数组
            staticBoxIDs = new uint[staticBoxTFs.Length];
            dynamicBoxIDs = new uint[dynamicBoxTFs.Length];
            dynamicCircleIDs = new uint[dynamicCircleTFs.Length];

            // 创建刚体
            for (int i = 0; i < staticBoxTFs.Length; i++) {
                var tf = staticBoxTFs[i];
                var rb = core.Rigidbody_CreateBox(tf.position.ToFVector2(), tf.localScale.ToFVector2());
                rb.SetIsStatic(true);
                rb.SetMass(0);
                tf.gameObject.name = $"Static_Box_{rb.ID}";
                var sr = tf.gameObject.GetComponent<SpriteRenderer>();
                sr.sprite = staticBoxSprite;
                sr.color = Color.green;
                staticBoxIDs[i] = rb.ID;
            }

            for (int i = 0; i < dynamicBoxTFs.Length; i++) {
                var tf = dynamicBoxTFs[i];
                var rb = core.Rigidbody_CreateBox(tf.position.ToFVector2(), tf.localScale.ToFVector2());
                rb.SetIsStatic(false);
                rb.SetVelocity(dynamicBoxVels[i].ToFVector2());
                rb.SetMass(0);
                tf.gameObject.name = $"Dynamic_Box_{rb.ID}";
                var sr = tf.gameObject.GetComponent<SpriteRenderer>();
                sr.sprite = dynamicBoxSprite;
                sr.color = Color.red;
                dynamicBoxIDs[i] = rb.ID;
            }

            for (int i = 0; i < dynamicCircleTFs.Length; i++) {
                var tf = dynamicCircleTFs[i];
                var rb = core.Rigidbody_CreateCircle(tf.position.ToFVector2(), tf.localScale.x / 2);
                rb.SetIsStatic(false);
                rb.SetVelocity(dynamicCircleVels[i].ToFVector2());
                rb.SetMass(0);
                tf.gameObject.name = $"Dynamic_Circle_{rb.ID}";
                var sr = tf.gameObject.GetComponent<SpriteRenderer>();
                sr.sprite = dynamicCircleSprite;
                sr.color = Color.red;
                dynamicCircleIDs[i] = rb.ID;
            }

            core.EventCenter.OnCollisionEnterHandle = (a, b) => { Debug.Log($"OnCollisionEnter: {a.ID} {b.ID}"); };
            core.EventCenter.OnCollisionExitHandle = (a, b) => { Debug.Log($"OnCollisionExit: {a.ID} {b.ID}"); };
            core.EventCenter.OnCollisionStayHandle = (a, b) => { Debug.Log($"OnCollisionStay: {a.ID} {b.ID}"); };

            core.EventCenter.OnTriggerEnterHandle = (a, b) => { Debug.Log($"OnTriggerEnter: {a.ID} {b.ID}"); };
            core.EventCenter.OnTriggerExitHandle = (a, b) => { Debug.Log($"OnTriggerExit: {a.ID} {b.ID}"); };
            core.EventCenter.OnTriggerStayHandle = (a, b) => { Debug.Log($"OnTriggerStay: {a.ID} {b.ID}"); };

        }

        void FixedUpdate() {
            core.Tick(Time.fixedDeltaTime);
        }

        void LateUpdate() {
            for (int i = 0; i < staticBoxTFs.Length; i++) {
                var tf = staticBoxTFs[i];
                var id = staticBoxIDs[i];
                var has = core.Rigidbody_TryGetByID(id, out var rb);
                if (!has) {
                    Debug.LogError($"StaticBox {id} not found");
                    continue;
                }
                tf.position = rb.Transform.Pos.ToVector3();
            }
            for (int i = 0; i < dynamicBoxTFs.Length; i++) {
                var tf = dynamicBoxTFs[i];
                var id = dynamicBoxIDs[i];
                var has = core.Rigidbody_TryGetByID(id, out var rb);
                if (!has) {
                    Debug.LogError($"DynamicBox {id} not found");
                    continue;
                }
                tf.position = rb.Transform.Pos.ToVector3();
                if (i == 0) {
                    var mat = new PhysicalMaterial();
                    mat.SetRestitution(1f);
                    rb.SetMaterial(mat);
                }
            }
            for (int i = 0; i < dynamicCircleTFs.Length; i++) {
                var tf = dynamicCircleTFs[i];
                var id = dynamicCircleIDs[i];
                var has = core.Rigidbody_TryGetByID(id, out var rb);
                if (!has) {
                    Debug.LogError($"DynamicCircle {id} not found");
                    continue;
                }
                tf.position = rb.Transform.Pos.ToVector3();
            }
        }

        void OnDrawGizmos() {
            for (int i = 0; i < staticBoxTFs.Length; i++) {
                var box = new BoxShape(staticBoxTFs[i].localScale.ToFVector2());
                var boxTF = new TFComponent(staticBoxTFs[i].position.ToFVector2());
                GizmosHeler.OnDrawBox(box, boxTF, Color.green);
                staticBoxTFs[i].gameObject.name = $"Static_Box_{i}";
            }

            for (int i = 0; i < dynamicBoxTFs.Length; i++) {
                var box = new BoxShape(dynamicBoxTFs[i].localScale.ToFVector2());
                var boxTF = new TFComponent(dynamicBoxTFs[i].position.ToFVector2());
                GizmosHeler.OnDrawBox(box, boxTF, Color.red);
                dynamicBoxTFs[i].gameObject.name = $"Dynamic_Box_{i}";
            }

            for (int i = 0; i < dynamicCircleTFs.Length; i++) {
                var circle = new CircleShape(dynamicCircleTFs[i].localScale.x / 2);
                var circleTF = new TFComponent(dynamicCircleTFs[i].position.ToFVector2());
                GizmosHeler.OnDrawCircle(circle, circleTF, Color.red);
                dynamicCircleTFs[i].gameObject.name = $"Dynamic_Circle_{i}";
            }

        }

    }

}