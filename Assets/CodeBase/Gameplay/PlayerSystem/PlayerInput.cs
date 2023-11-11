using System;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerInput : ITickable
    {
        public event Action<Vector3> Pressed;

        public void Tick()
        {
            var horizontalInput = SimpleInput.GetAxisRaw("Horizontal");
            var verticalInput = SimpleInput.GetAxisRaw("Vertical");
            var moveVector = new Vector3(horizontalInput, 0, verticalInput);
            Pressed?.Invoke(moveVector.normalized);
        }
    }
}