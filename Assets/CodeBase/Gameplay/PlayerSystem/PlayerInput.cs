using System;
using CodeBase.Services.Input;
using CodeBase.Services.Providers.Camera;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerInput : ITickable
    {
        public event Action<Vector3> MovementPressed;

        private CameraProvider _cameraProvider;

        public PlayerInput(CameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
        }

        public void Tick()
        {
            if(_cameraProvider.Camera == null)
                return;
            
            var horizontalInput = SimpleInput.GetAxisRaw("Horizontal");
            var verticalInput = SimpleInput.GetAxisRaw("Vertical");

            Vector3 cameraForward = _cameraProvider.Camera.transform.forward;
            Vector3 cameraRight = _cameraProvider.Camera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            Vector3 forwardRelative = verticalInput * cameraForward;
            Vector3 rightRelative = horizontalInput * cameraRight;

            Vector3 moveDirection = forwardRelative + rightRelative;
            
            MovementPressed?.Invoke(new Vector3(moveDirection.x, 0, moveDirection.z));
        }
    }
}