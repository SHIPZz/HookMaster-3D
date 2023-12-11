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
            var horizontalInput = SimpleInput.GetAxisRaw("Horizontal");
            var verticalInput = SimpleInput.GetAxisRaw("Vertical");
            
            MovementPressed?.Invoke(new Vector3(horizontalInput, 0, verticalInput));

        }
    }
}