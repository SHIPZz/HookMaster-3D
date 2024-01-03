using System;
using UnityEngine;

namespace CodeBase.Services.TriggerObserve
{
    public class TriggerObserver : MonoBehaviour
    {
        public event Action<Collider> TriggerEntered;
        public event Action<Collider> TriggerExited;
        public event Action<Collision> CollisionEntered;
        public event Action<Collision> CollisionExited;

        private void OnTriggerEnter(Collider other)
        {
            TriggerEntered?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExited?.Invoke(other);
        }

        private void OnCollisionEnter(Collision other)
        {
            CollisionEntered?.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            CollisionExited?.Invoke(other);
        }
    }
}