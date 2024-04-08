using System;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerMovementMediator : IInitializable, IDisposable
    {
        private readonly PlayerInput _playerInput;
        private readonly PlayerMovement _playerMovement;

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