using System;
using UnityEngine;

namespace CodeBase.Gameplay.AnimMovement
{
    public class ResourceCurveMovement : AfterResourceCreateMovementBehaviour
    {
        [SerializeField] private AnimationCurve _animationCurve;
        
        public override void Move(GameObject target, Vector3 startPosition, Func<Vector3> finalPositionProvider, float speed, Action onComplete = null)
        {
            var movement = target.AddComponent<AnimationCurveMovement>();
            Vector3 finalPosition = finalPositionProvider.Invoke();
            movement.Initialize(_animationCurve, onComplete);
            movement.Move(startPosition, finalPosition, speed);
        }
    }
}