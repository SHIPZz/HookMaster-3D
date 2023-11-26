using System;
using CodeBase.Services.Providers;
using CodeBase.Services.Providers.Camera;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerMovementMediator : IInitializable, IDisposable
    {
        private readonly PlayerInput _playerInput;
        private readonly PlayerMovement _playerMovement;
        private CameraProvider _cameraProvider;

        public PlayerMovementMediator(PlayerInput playerInput, PlayerMovement playerMovement)
        {
            _playerInput = playerInput;
            _playerMovement = playerMovement;
        }

        public void Initialize()
        {
            _playerInput.MovementPressed += _playerMovement.SetMovement;
        }

        public void Dispose()
        {
            _playerInput.MovementPressed -= _playerMovement.SetMovement;
        }
    }
}