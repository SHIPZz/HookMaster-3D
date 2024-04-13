using System;
using CodeBase.Gameplay.ResourceItem;
using UnityEngine;

namespace CodeBase.Gameplay.AnimMovement
{
    public class ResourceCurveMovement : AfterResourceCreateMovementBehaviour
    {
        [SerializeField] private AnimationCurve _animationCurve;
        
        public override void Move(Resource target, Vector3 startPosition, Func<Vector3> finalPositionProvider, float speed, Action onComplete = null)
        {
            target.MovementCompleted = false;
            var movement = target.gameObject.AddComponent<AnimationCurveMovement>();
            target.TrackMovementFinish(movement);
            Vector3 finalPosition = finalPositionProvider.Invoke();
            movement.Initialize(_animationCurve, onComplete);
            movement.Move(startPosition, finalPosition, speed);
        }
    }
}