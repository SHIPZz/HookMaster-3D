using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Animations
{
    public class PositionLoopAnim : MonoBehaviour
    {
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Vector3 _targetPosition;
        
        private Tween _animationTween;
        private Vector3 _initialPosition;

        private void Awake()
        {
            _initialPosition = transform.position;
        }

        private void OnEnable()
        {
            StartAnimation();
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        private void StartAnimation()
        {
            _animationTween = transform.DOMove(_initialPosition + _targetPosition, _duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true);
        }

        private void StopAnimation()
        {
            _animationTween?.Kill();
            _animationTween = null;
        }
    }
}