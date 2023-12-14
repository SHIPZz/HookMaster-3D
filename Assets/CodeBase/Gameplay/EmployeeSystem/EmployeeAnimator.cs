using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
