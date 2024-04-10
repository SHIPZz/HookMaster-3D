using System;
using CodeBase.Gameplay.ResourceItem;
using UnityEngine;

namespace CodeBase.Gameplay.AnimMovement
{
    public abstract class AfterResourceCreateMovementBehaviour : MonoBehaviour
    {
        public abstract void Move(Resource target, Vector3 startPosition, Func<Vector3> finalPositionProvider, float speed, Action onComplete = null);
    }
}