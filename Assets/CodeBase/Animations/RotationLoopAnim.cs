using DG.Tweening;
using UnityEngine;

namespace CodeBase.Animations
{
    public class RotationLoopAnim : MonoBehaviour
    {
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Vector3 _targetRotation;
        [SerializeField] private LoopType _loopType;
        
        private Tween _animationTween;

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
            print("startAnim");
            _animationTween = transform.DORotate(_targetRotation, _duration, RotateMode.FastBeyond360)
                .SetLoops(-1, _loopType)
                .SetEase(Ease.Linear)
                .SetUpdate(true);
        }

        private void StopAnimation()
        {
            _animationTween?.Kill();
            _animationTween = null;
        }
    }
}