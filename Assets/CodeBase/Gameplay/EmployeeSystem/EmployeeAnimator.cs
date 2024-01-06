using UnityEngine;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class EmployeeAnimator
    {
        private static readonly int _speed = Animator.StringToHash("Speed");
        private static readonly int _isSitTyping = Animator.StringToHash("IsSitTyping");
        private readonly Animator _animator;

        public EmployeeAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void SetSitTyping(bool isSitTyping)
        {
            _animator.SetBool(_isSitTyping, isSitTyping);
        }

        public void SetSpeed(float speed)
        {
            _animator.SetFloat(_speed, speed);
        }
    }
}
