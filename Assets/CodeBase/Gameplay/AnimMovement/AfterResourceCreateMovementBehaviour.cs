using System;
using UnityEngine;

namespace CodeBase.Gameplay.AnimMovement
{
    public abstract class AfterResourceCreateMovementBehaviour : MonoBehaviour
    {
        public abstract void Move(GameObject target, Vector3 startPosition, Func<Vector3> finalPositionProvider, float speed);
    }
}