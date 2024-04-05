using System;
using CodeBase.Services.CameraServices;
using CodeBase.Services.Input;
using CodeBase.Services.Providers.Camera;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerInput : ITickable
    {
        public event Action<Vector3> MovementPressed;

        private readonly CameraController _cameraController;
        private bool _isBlocked = false;

        public PlayerInput(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        public void Tick()
        {
            if(_cameraController.Camera == null || _isBlocked)
                return;
            
            var horizontalInput = SimpleInput.GetAxisRaw("Horizontal");
            var verticalInput = SimpleInput.GetAxisRaw("Vertical");

            Vector3 cameraForward = _cameraController.Camera.transform.forward;
            Vector3 cameraRight = _cameraController.Camera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            Vector3 forwardRelative = verticalInput * cameraForward;
            Vector3 rightRelative = horizontalInput * cameraRight;

            Vector3 moveDirection = (forwardRelative + rightRelative).normalized;
            
            MovementPressed?.Invoke(new Vector3(moveDirection.x, 0, moveDirection.z));
        }

        public void SetBlocked(bool isBlocked)
        {
            _isBlocked = isBlocked;
            MovementPressed?.Invoke(Vector3.zero);
        }
    }
}