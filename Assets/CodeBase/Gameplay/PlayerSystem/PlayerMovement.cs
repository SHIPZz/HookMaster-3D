using System;
using UnityEngine;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _rotationSpeed = 25f;
        [SerializeField] private Rigidbody _rigidbody;

        private Vector3 _normalizedDirection;

        private void FixedUpdate()
        {
            if (_normalizedDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_normalizedDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }

            _rigidbody.velocity = _normalizedDirection * _speed * Time.deltaTime;
        }

        public void SetMovement(Vector3 normalizedDirection)
        {
            _normalizedDirection = normalizedDirection;
        }
    }
}