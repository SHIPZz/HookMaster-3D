using System;
using UnityEngine;

namespace CodeBase.Gameplay.AnimMovement
{
    public class ResourceCurveMovement : AfterResourceCreateMovementBehaviour
    {
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private Transform _startPosition;
        
        public override void Move(GameObject target, Vector3 startPosition, Func<Vector3> finalPositionProvider, float speed, Action onComplete = null)
        {
            if (_startPosition != null)
                startPosition = _startPosition.position;

            var movement = target.AddComponent<AnimationCurveMovement>();
            Vector3 finalPosition = finalPositionProvider.Invoke();
            movement.Initialize(_animationCurve, onComplete);
            movement.Move(_startPosition.localPosition, finalPosition, speed);
        }
    }
}