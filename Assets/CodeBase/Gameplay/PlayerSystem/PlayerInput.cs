using System;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerInput : ITickable
    {
        public event Action<Vector3> MovementPressed;

        public void Tick()
        {
            var horizontalInput = SimpleInput.GetAxisRaw("Horizontal");
            var verticalInput = SimpleInput.GetAxisRaw("Vertical");
            
            MovementPressed?.Invoke(new Vector3(horizontalInput, 0, verticalInput));
        }
    }
}