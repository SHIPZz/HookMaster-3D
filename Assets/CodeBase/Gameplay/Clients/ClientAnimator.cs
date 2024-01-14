using UnityEngine;

namespace CodeBase.Gameplay.Clients
{
    public class ClientAnimator
    {
        private static readonly int _speed = Animator.StringToHash("Speed");
        private static readonly int _sitIdle = Animator.StringToHash("IsSitIdle");
        private readonly Animator _animator;

        public ClientAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void SetIsSitIdle(bool isSitIdle)
        {
            _animator.SetBool(_sitIdle, isSitIdle);
        }

        public void SetSpeed(float speed)
        {
            _animator.SetFloat(_speed,speed);
        }
    }
}