using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Services.Input
{
    public class InputService : IInputService
    {
        public DefaultInputActions PlayerInputActions { get; }

        public InputService()
        {
            PlayerInputActions = new DefaultInputActions();
            PlayerInputActions.Enable();
        }

        public Vector2 PointPosition() => PlayerInputActions.UI.Point.ReadValue<Vector2>();
    }
}