using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Services.Input
{
    public interface IInputService
    {
        DefaultInputActions PlayerInputActions { get; }
        Vector2 PointPosition();
    }
}