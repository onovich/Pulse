using System;
using System.Collections.Generic;
using System.Linq;
using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    public class PhysicalContext {

        // Gravity
        Vector2 gravity;
        public Vector2 Gravity => gravity;

        // Event
        PhysicalEventCenter eventCenter;
        public PhysicalEventCenter EventCenter => eventCenter;

        // Repo
        SortedList<uint, RigidbodyEntity> rigidbodies;
        RigidbodyEntity[] tempRigidBodyArray;

        // Event Queue
        Queue<(RigidbodyEntity, RigidbodyEntity)> collisionEnterQueue;
        Queue<(RigidbodyEntity, RigidbodyEntity)> collisionStayQueue;
        Queue<(RigidbodyEntity, RigidbodyEntity)> collisionExitQueue;

        Queue<(RigidbodyEntity, RigidbodyEntity)> triggerEnterQueue;
        Queue<(RigidbodyEntity, RigidbodyEntity)> triggerStayQueue;
        Queue<(RigidbodyEntity, RigidbodyEntity)> triggerExitQueue;

        // Contact
        Dictionary<ulong, (ulong, RigidbodyEntity, RigidbodyEntity)> collisionContacts;

        public PhysicalContext() {
            rigidbodies = new SortedList<uint, RigidbodyEntity>();
            tempRigidBodyArray = new RigidbodyEntity[0];
            eventCenter = new PhysicalEventCenter();
            collisionEnterQueue = new Queue<(RigidbodyEntity, RigidbodyEntity)>();
            collisionStayQueue = new Queue<(RigidbodyEntity, RigidbodyEntity)>();
            collisionExitQueue = new Queue<(RigidbodyEntity, RigidbodyEntity)>();
            triggerEnterQueue = new Queue<(RigidbodyEntity, RigidbodyEntity)>();
            triggerStayQueue = new Queue<(RigidbodyEntity, RigidbodyEntity)>();
            triggerExitQueue = new Queue<(RigidbodyEntity, RigidbodyEntity)>();
            collisionContacts = new Dictionary<ulong, (ulong, RigidbodyEntity, RigidbodyEntity)>();
        }

        // Rigidbody
        public void Rigidbody_Add(RigidbodyEntity rb) {
            rigidbodies.Add(rb.ID, rb);
        }

        public int Rigidbody_TakeAll(out RigidbodyEntity[] res) {
            int count = rigidbodies.Count;
            if (count > tempRigidBodyArray.Length) {
                tempRigidBodyArray = new RigidbodyEntity[(int)(count * 1.5f)];
            }
            rigidbodies.Values.CopyTo(tempRigidBodyArray, 0);
            res = tempRigidBodyArray;
            return count;
        }

        public void Rigidbody_Remove(RigidbodyEntity rb) {
            rigidbodies.Remove(rb.ID);
        }

        public void Rigidbody_ForEach(Action<RigidbodyEntity> action) {
            foreach (var rb in rigidbodies.Values) {
                action.Invoke(rb);
            }
        }

        // Gravity
        public void SetGravity(Vector2 value) {
            gravity = value;
        }

        // Collision
        public void EnqueueCollisionEnter(RigidbodyEntity a, RigidbodyEntity b) {
            collisionEnterQueue.Enqueue((a, b));
        }

        public void EnqueueCollisionStay(RigidbodyEntity a, RigidbodyEntity b) {
            collisionStayQueue.Enqueue((a, b));
        }

        public void EnqueueCollisionExit(RigidbodyEntity a, RigidbodyEntity b) {
            collisionExitQueue.Enqueue((a, b));
        }

        public bool TryDequeueCollisionEnter(out RigidbodyEntity a, out RigidbodyEntity b) {
            if (collisionEnterQueue.TryDequeue(out var pair)) {
                (a, b) = pair;
                return true;
            } else {
                a = null;
                b = null;
                return false;
            }
        }

        public bool TryDequeueCollisionStay(out RigidbodyEntity a, out RigidbodyEntity b) {
            if (collisionStayQueue.TryDequeue(out var pair)) {
                (a, b) = pair;
                return true;
            } else {
                a = null;
                b = null;
                return false;
            }
        }

        public bool TryDequeueCollisionExit(out RigidbodyEntity a, out RigidbodyEntity b) {
            if (collisionExitQueue.TryDequeue(out var pair)) {
                (a, b) = pair;
                return true;
            } else {
                a = null;
                b = null;
                return false;
            }
        }

        // Trigger
        public void EnqueueTriggerEnter(RigidbodyEntity a, RigidbodyEntity b) {
            triggerEnterQueue.Enqueue((a, b));
        }

        public void EnqueueTriggerStay(RigidbodyEntity a, RigidbodyEntity b) {
            triggerStayQueue.Enqueue((a, b));
        }

        public void EnqueueTriggerExit(RigidbodyEntity a, RigidbodyEntity b) {
            triggerExitQueue.Enqueue((a, b));
        }

        public bool TryDequeueTriggerEnter(out RigidbodyEntity a, out RigidbodyEntity b) {
            if (triggerEnterQueue.TryDequeue(out var pair)) {
                (a, b) = pair;
                return true;
            } else {
                a = null;
                b = null;
                return false;
            }
        }

        public bool TryDequeueTriggerStay(out RigidbodyEntity a, out RigidbodyEntity b) {
            if (triggerStayQueue.TryDequeue(out var pair)) {
                (a, b) = pair;
                return true;
            } else {
                a = null;
                b = null;
                return false;
            }
        }

        public bool TryDequeueTriggerExit(out RigidbodyEntity a, out RigidbodyEntity b) {
            if (triggerExitQueue.TryDequeue(out var pair)) {
                (a, b) = pair;
                return true;
            } else {
                a = null;
                b = null;
                return false;
            }
        }

        // Contact
        public void CollisionContact_Add(RigidbodyEntity a, RigidbodyEntity b) {
            ulong key = IDService.ContactKey(a.ID, b.ID);
            collisionContacts[key] = (key, a, b);
        }

        public bool CollisionContact_TryGet(RigidbodyEntity a, RigidbodyEntity b, out (ulong, RigidbodyEntity, RigidbodyEntity) value) {
            ulong key = IDService.ContactKey(a.ID, b.ID);
            return collisionContacts.TryGetValue(key, out value);
        }

        public bool CollisionContact_Remove(RigidbodyEntity a, RigidbodyEntity b) {
            ulong key = IDService.ContactKey(a.ID, b.ID);
            return collisionContacts.Remove(key);
        }

        public bool CollisionContact_Contains(RigidbodyEntity a, RigidbodyEntity b) {
            ulong key = IDService.ContactKey(a.ID, b.ID);
            return collisionContacts.ContainsKey(key);
        }

        public KeyValuePair<ulong, (ulong, RigidbodyEntity, RigidbodyEntity)>[] CollisionContact_TakeAll() {
            return collisionContacts.ToArray();
        }

        public void CollisionContact_Clear() {
            collisionContacts.Clear();
        }

    }

}