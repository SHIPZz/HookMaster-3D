using System;
using UnityEngine;

namespace CodeBase.Gameplay.BulletSystem
{
    public class Bullet : MonoBehaviour
    {
        private BulletMovement _bulletMovement;

        private void Awake()
        {
            _bulletMovement = GetComponent<BulletMovement>();
        }

        public void StartMoving(Vector3 direction, Vector3 startPosition)
        {
            _bulletMovement.Move(direction,startPosition);
        }
    }
}