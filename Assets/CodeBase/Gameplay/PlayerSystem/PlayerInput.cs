using System;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerInput : ITickable
    {
        private readonly IInputService _inputService;
        public event Action<Vector3> MovementPressed;

        public PlayerInput(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Tick()
        {
            // if(!_inputService.PlayerInputActions.UI.Click.WasReleasedThisFrame())
            //     return;
            //
            // Vector2 pointPosition = _inputService.PointPosition();
            // MousePressed?.Invoke(pointPosition);
            
            var horizontalInput = SimpleInput.GetAxisRaw("Horizontal");
            var verticalInput = SimpleInput.GetAxisRaw("Vertical");

            // var horizontalInput = UltimateJoystick.GetHorizontalAxisRaw("Movement");
            // var verticalInput = UltimateJoystick.GetVerticalAxis("Movement");

            MovementPressed?.Invoke(new Vector3(horizontalInput, 0, verticalInput));

        }
    }
}