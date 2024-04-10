using System;
using CodeBase.Gameplay.AnimMovement;
using CodeBase.Gameplay.GameItems;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.ResourceItem
{
    public class Resource : GameItemAbstract
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private bool _needDestroy = true;
        [SerializeField] private bool _setParent;
        [SerializeField] private float _destroyDelay = 0.2f;
        [field: SerializeField] public bool NeedChangeScale { get; private set; }

        public bool IsCollected { get; private set; }
        
        public bool MovementCompleted { get; set; }

        public event Action<Resource> Collected;

        public void MarkAsDetected()
        {
            _collider.enabled = false;
        }

        public void MoveTo(Vector3 target)
        {
            transform.DOLocalMove(target, 0.5f).SetEase(Ease.InOutQuint)
                .OnComplete(() => MovementCompleted = true);
        }

        public virtual void Collect(Transform parent)
        {
            Collected?.Invoke(this);

            if (_setParent)
            {
                transform.SetParent(parent);
                transform.localRotation = Quaternion.identity;
                transform.localPosition = Vector3.zero;
            }

            IsCollected = true;
            
            if (_needDestroy)
                Destroy(gameObject,_destroyDelay);
        }

        public void TrackMovementFinish(AnimationCurveMovement movement)
        {
            movement.MovementCompleted += SetMovementCompletedHandler;
        }

        private void SetMovementCompletedHandler(AnimationCurveMovement animationCurveMovement)
        {
            MovementCompleted = true;
            animationCurveMovement.MovementCompleted -= SetMovementCompletedHandler;
        }
    }
}