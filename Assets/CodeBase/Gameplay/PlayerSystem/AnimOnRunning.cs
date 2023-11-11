using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class AnimOnRunning : MonoBehaviour
    {
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
            if (_rigidbody.velocity.z > 0.1f && _rigidbody.velocity.y < 1f)
            {
                _playerAnimator.SetSpeed(1f);
                return;
            }
            
            _playerAnimator.SetSpeed(0f);
        }
    }
}