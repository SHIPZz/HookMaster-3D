using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class AnimOnMoving : MonoBehaviour
    {
        private const float MinimalVelocity = 0.1f;
        private PlayerAnimator _playerAnimator;
        private Rigidbody _rigidbody;

        [Inject]
        private void Construct(PlayerAnimator playerAnimator, Rigidbody rigidbody)
        {
            _playerAnimator = playerAnimator;
            _rigidbody = rigidbody;
        }

        public void FixedUpdate()
        {
            if (_rigidbody.velocity.magnitude > MinimalVelocity)
            {
                _playerAnimator.SetSpeed(1f);
                return;
            }

            _playerAnimator.SetSpeed(0f);
        }
    }
}