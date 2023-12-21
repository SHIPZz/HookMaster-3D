using UnityEngine;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerAnimator
    {
        private static readonly int _speed = Animator.StringToHash("Speed");
        private static readonly int _isPunching = Animator.StringToHash("IsPunching");
        private readonly Animator _animator;

        public PlayerAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void SetSpeed(float speed)
        {
            _animator.SetFloat(_speed, speed);
        }

        public void SetPunch(bool isPunching)
        {
            _animator.SetBool(_isPunching, isPunching);
        }
    }
}