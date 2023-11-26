using System;
using UnityEngine;

namespace CodeBase.Services.TriggerObserve
{
    public class TriggerObserver : MonoBehaviour
    {
        public event Action<Collider> TriggerEntered;
        public event Action<Collision> CollisionEntered;

        private void OnTriggerEnter(Collider other)
        {
            TriggerEntered?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerEntered?.Invoke(other);
        }

        private void OnCollisionEnter(Collision other)
        {
            print(other.gameObject.name);
            CollisionEntered?.Invoke(other);
        }
    }
}