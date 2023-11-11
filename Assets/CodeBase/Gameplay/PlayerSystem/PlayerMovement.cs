using System;
using UnityEngine;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _forwardSpeed = 5f;
        [SerializeField] private float _jumpForce = 8f;
        [SerializeField] private float _gravity = 25f;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Transform _legPosition;
        [SerializeField] private float _maxRotationAngle = 45f;

        private bool _isJumped;
        private Camera _camera;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_isJumped && IsGrounded())
            {
                Jump();
                return;
            }

            _isJumped = false;
        }

        private void FixedUpdate()
        {
            ApplyGravity();

            MoveForward();
        }

        private void MoveForward() => 
            _rigidbody.MovePosition(_rigidbody.position + transform.forward * _forwardSpeed * Time.fixedDeltaTime);

        private void ApplyGravity() => 
            _rigidbody.velocity += Vector3.down * _gravity * Time.fixedDeltaTime;

        private void Jump()
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _isJumped = true;
        }

        public void SetCamera(Camera camera) =>
            _camera = camera;

        public void SetMovement(Vector3 normalizedDirection)
        {
            Vector3 cameraForward = _camera.transform.forward;

            var angle = Vector3.Angle(cameraForward, normalizedDirection);

            if (angle > _maxRotationAngle)
            {
                var dot = Vector3.Dot(normalizedDirection, _camera.transform.right);
                normalizedDirection = new Vector3(dot, 0, 1f).normalized;
            }

            transform.rotation = Quaternion.FromToRotation(cameraForward, normalizedDirection);
        }

        private bool IsGrounded() =>
            Physics.Raycast(_legPosition.position, Vector3.down * 2f, 0.8f, _layerMask);
    }
}