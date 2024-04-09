using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Gameplay.AnimMovement
{
    public class AnimationCurveMovement : MonoBehaviour
    {
        private float _animationDuration = 1f;
        private AnimationCurve _animationCurve;
        
        private Coroutine _moveCoroutine;
        private bool _destroyOnReachTarget;
        private Action _onTargetReached;

        public void Initialize(AnimationCurve animationCurve, Action onTargetReached = null)
        {
            _onTargetReached = onTargetReached;
            _animationCurve = animationCurve;
        }
        
        public void Move(Vector3 startPosition, Vector3 finalPosition, float duration)
        {
            if (_moveCoroutine == null)
            {
                _animationDuration = duration;
                _moveCoroutine = StartCoroutine(MoveObject(startPosition, finalPosition));
            }
        }

        private IEnumerator MoveObject(Vector3 startPosition, Vector3 finalPosition)
        {
            transform.localPosition = startPosition;

            var elapsedTime = 0f;

            while (Vector3.Distance(transform.localPosition, finalPosition) > 0.1f)
            {
                float evalTime = elapsedTime / _animationDuration;
                evalTime = Mathf.Clamp01(evalTime);
                float height = _animationCurve.Evaluate(evalTime);

                var newPosition = Vector3.Lerp(startPosition, finalPosition, evalTime);
                newPosition.y += height;

                transform.localPosition = newPosition;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            _moveCoroutine = null;
            _onTargetReached?.Invoke();
        }
    }
}