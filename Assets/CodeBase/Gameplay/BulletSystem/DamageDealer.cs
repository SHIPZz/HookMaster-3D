using System;
using UnityEngine;

namespace CodeBase.Gameplay.BulletSystem
{
    public class DamageDealer : MonoBehaviour
    {
        private float _damage;

        private void OnCollisionEnter(Collision other)
        {
            
        }

        public void SetDamage(float damage) =>
            _damage = damage;
    }
}