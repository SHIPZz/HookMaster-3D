using System;
using CodeBase.Gameplay.GameItems;
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
        
        public event Action<Resource> Collected;
        

        public void MarkAsDetected()
        {
            _collider.enabled = false;
        }
        
        public void Collect()
        {
            Collected?.Invoke(this);

            if (_needDestroy)
                Destroy(gameObject);
        }
        
        public virtual void Collect(Transform parent)
        {
            Collected?.Invoke(this);

            if (_setParent)
            {
                transform.SetParent(parent);
                transform.localRotation = Quaternion.identity;
            }

            IsCollected = true;
            
            if (_needDestroy)
                Destroy(gameObject,_destroyDelay);
        }
    }
}