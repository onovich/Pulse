using System;
using System.Collections.Generic;
using System.Linq;
using MortiseFrame.Abacus;

namespace MortiseFrame.Pulse {

    internal class PhysicalContext {

        // Gravity
        FVector2 gravity;
        internal FVector2 Gravity => gravity;

        // Event
        PhysicalEventCenter eventCenter;
        internal PhysicalEventCenter EventCenter => eventCenter;

        // Repo
        Dictionary<uint, RigidbodyEntity> rigidbodies;
        RigidbodyEntity[] tempRigidBodyArray;

        // Event Queue
        Queue<(RigidbodyEntity, RigidbodyEntity)> collisionEnterQueue;
        Queue<(RigidbodyEntity, RigidbodyEntity)> collisionStayQueue;
        Queue<(RigidbodyEntity, RigidbodyEntity)> collisionExitQueue;

        Queue<(RigidbodyEntity, RigidbodyEntity)> triggerEnterQueue;
        Queue<(RigidbodyEntity, RigidbodyEntity)> triggerStayQueue;
        Queue<(RigidbodyEntity, RigidbodyEntity)> triggerExitQueue;

        // Contact
        Dictionary<ulong, (ulong, RigidbodyEntity, RigidbodyEntity)> intersectContacts;
        Dictionary<ulong, (ulong, RigidbodyEntity, RigidbodyEntity)> collisionContacts;

        // Ignore
        HashSet<ulong> ignoreDict;

        internal PhysicalContext() {
            rigidbodies = new Dictionary<uint, RigidbodyEntity>();
            tempRigidBodyArray = new RigidbodyEntity[0];
            eventCenter = new PhysicalEventCenter();
            collisionEnterQueue = new Queue<(RigidbodyEntity, RigidbodyEntity)>();
            collisionStayQueue = new Queue<(RigidbodyEntity, RigidbodyEntity)>();
            collisionExitQueue = new Queue<(RigidbodyEntity, RigidbodyEntity)>();
            triggerEnterQueue = new Queue<(RigidbodyEntity, RigidbodyEntity)>();
            triggerStayQueue = new Queue<(RigidbodyEntity, RigidbodyEntity)>();
            triggerExitQueue = new Queue<(RigidbodyEntity, RigidbodyEntity)>();
            collisionContacts = new Dictionary<ulong, (ulong, RigidbodyEntity, RigidbodyEntity)>();
            intersectContacts = new Dictionary<ulong, (ulong, RigidbodyEntity, RigidbodyEntity)>();
            ignoreDict = new HashSet<ulong>();
        }

        // Rigidbody
        internal void Rigidbody_Add(RigidbodyEntity rb) {
            rigidbodies.Add(rb.ID, rb);
        }

        internal bool Rigidbody_TryGetByID(uint id, out RigidbodyEntity res) {
            return rigidbodies.TryGetValue(id, out res);
        }

        internal int Rigidbody_TakeAll(out RigidbodyEntity[] res) {
            int count = rigidbodies.Count;
            if (count > tempRigidBodyArray.Length) {
                tempRigidBodyArray = new RigidbodyEntity[(int)(count * 1.5f)];
            }
            rigidbodies.Values.CopyTo(tempRigidBodyArray, 0);
            res = tempRigidBodyArray;
            return count;
        }

        internal void Rigidbody_Remove(RigidbodyEntity rb) {
            rigidbodies.Remove(rb.ID);
        }

        internal void Rigidbody_ForEach(Action<RigidbodyEntity> action) {
            foreach (var rb in rigidbodies.Values) {
                action.Invoke(rb);
            }
        }

        // Gravity
        internal void SetGravity(FVector2 value) {
            gravity = value;
        }

        // Collision
        internal void EnqueueCollisionEnter(RigidbodyEntity a, RigidbodyEntity b) {
            collisionEnterQueue.Enqueue((a, b));
        }

        internal void EnqueueCollisionStay(RigidbodyEntity a, RigidbodyEntity b) {
            collisionStayQueue.Enqueue((a, b));
        }

        internal void EnqueueCollisionExit(RigidbodyEntity a, RigidbodyEntity b) {
            collisionExitQueue.Enqueue((a, b));
        }

        internal bool TryDequeueCollisionEnter(out RigidbodyEntity a, out RigidbodyEntity b) {
            if (collisionEnterQueue.TryDequeue(out var pair)) {
                (a, b) = pair;
                return true;
            } else {
                a = null;
                b = null;
                return false;
            }
        }

        internal bool TryDequeueCollisionStay(out RigidbodyEntity a, out RigidbodyEntity b) {
            if (collisionStayQueue.TryDequeue(out var pair)) {
                (a, b) = pair;
                return true;
            } else {
                a = null;
                b = null;
                return false;
            }
        }

        internal bool TryDequeueCollisionExit(out RigidbodyEntity a, out RigidbodyEntity b) {
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
        internal void EnqueueTriggerEnter(RigidbodyEntity a, RigidbodyEntity b) {
            triggerEnterQueue.Enqueue((a, b));
        }

        internal void EnqueueTriggerStay(RigidbodyEntity a, RigidbodyEntity b) {
            triggerStayQueue.Enqueue((a, b));
        }

        internal void EnqueueTriggerExit(RigidbodyEntity a, RigidbodyEntity b) {
            triggerExitQueue.Enqueue((a, b));
        }

        internal bool TryDequeueTriggerEnter(out RigidbodyEntity a, out RigidbodyEntity b) {
            if (triggerEnterQueue.TryDequeue(out var pair)) {
                (a, b) = pair;
                return true;
            } else {
                a = null;
                b = null;
                return false;
            }
        }

        internal bool TryDequeueTriggerStay(out RigidbodyEntity a, out RigidbodyEntity b) {
            if (triggerStayQueue.TryDequeue(out var pair)) {
                (a, b) = pair;
                return true;
            } else {
                a = null;
                b = null;
                return false;
            }
        }

        internal bool TryDequeueTriggerExit(out RigidbodyEntity a, out RigidbodyEntity b) {
            if (triggerExitQueue.TryDequeue(out var pair)) {
                (a, b) = pair;
                return true;
            } else {
                a = null;
                b = null;
                return false;
            }
        }

        // Collision Contact
        internal void CollisionContact_Add(RigidbodyEntity a, RigidbodyEntity b) {
            ulong key = IDService.ContactKey(a.ID, b.ID);
            collisionContacts[key] = (key, a, b);
        }

        internal bool CollisionContact_TryGet(RigidbodyEntity a, RigidbodyEntity b, out (ulong, RigidbodyEntity, RigidbodyEntity) value) {
            ulong key = IDService.ContactKey(a.ID, b.ID);
            return collisionContacts.TryGetValue(key, out value);
        }

        internal bool CollisionContact_Remove(RigidbodyEntity a, RigidbodyEntity b) {
            ulong key = IDService.ContactKey(a.ID, b.ID);
            return collisionContacts.Remove(key);
        }

        internal bool CollisionContact_Contains(RigidbodyEntity a, RigidbodyEntity b) {
            ulong key = IDService.ContactKey(a.ID, b.ID);
            return collisionContacts.ContainsKey(key);
        }

        internal void CollisionContact_ForEach(Action<(ulong, RigidbodyEntity, RigidbodyEntity)> action) {
            foreach (var pair in collisionContacts.Values) {
                action.Invoke(pair);
            }
        }

        internal KeyValuePair<ulong, (ulong, RigidbodyEntity, RigidbodyEntity)>[] CollisionContact_GetAll() {
            return collisionContacts.ToArray();
        }

        internal void CollisionContact_Clear() {
            collisionContacts.Clear();
        }

        // Intersect Contact
        internal void IntersectContact_Add(RigidbodyEntity a, RigidbodyEntity b) {
            ulong key = IDService.ContactKey(a.ID, b.ID);
            intersectContacts[key] = (key, a, b);
        }

        internal bool IntersectContact_TryGet(RigidbodyEntity a, RigidbodyEntity b, out (ulong, RigidbodyEntity, RigidbodyEntity) value) {
            ulong key = IDService.ContactKey(a.ID, b.ID);
            return intersectContacts.TryGetValue(key, out value);
        }

        internal bool IntersectContact_Remove(RigidbodyEntity a, RigidbodyEntity b) {
            ulong key = IDService.ContactKey(a.ID, b.ID);
            return intersectContacts.Remove(key);
        }

        internal bool IntersectContact_Contains(RigidbodyEntity a, RigidbodyEntity b) {
            ulong key = IDService.ContactKey(a.ID, b.ID);
            return intersectContacts.ContainsKey(key);
        }

        internal void IntersectContact_ForEach(Action<(ulong, RigidbodyEntity, RigidbodyEntity)> action) {
            foreach (var pair in intersectContacts.Values) {
                action.Invoke(pair);
            }
        }

        internal KeyValuePair<ulong, (ulong, RigidbodyEntity, RigidbodyEntity)>[] IntersectContact_GetAll() {
            return intersectContacts.ToArray();
        }

        internal void IntersectContact_Clear() {
            intersectContacts.Clear();
        }

        // Ignore
        internal void Ignore_Add(uint layerA, uint layerB) {
            ulong key = IDService.ContactKey(layerA, layerB);
            ignoreDict.Add(key);
        }

        internal void Ignore_Remove(uint layerA, uint layerB) {
            ulong key = IDService.ContactKey(layerA, layerB);
            ignoreDict.Remove(key);
        }

        internal bool Ignore_Contains(uint layerA, uint layerB) {
            ulong key = IDService.ContactKey(layerA, layerB);
            return ignoreDict.Contains(key);
        }

        internal void Clear() {
            rigidbodies.Clear();
            collisionEnterQueue.Clear();
            collisionStayQueue.Clear();
            collisionExitQueue.Clear();
            triggerEnterQueue.Clear();
            triggerStayQueue.Clear();
            triggerExitQueue.Clear();
            collisionContacts.Clear();
            intersectContacts.Clear();
            eventCenter.Clear();
        }

    }

}