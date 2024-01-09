using System;
using System.Collections.Generic;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.Fire
{
    public class FireSystem : MonoBehaviour
    {
        [SerializeField] private float _putOutHitCount = 5f;
        [SerializeField] private List<ParticleSystem> _fires;
        [SerializeField] private TriggerObserver _triggerObserver;

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnSmoked;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnSmoked;
        }

        private void OnSmoked(Collider obj)
        {
            _putOutHitCount = Mathf.Clamp(_putOutHitCount--, 0, _putOutHitCount);
            
            if(_putOutHitCount == 0)
                _fires.ForEach(x => x.Stop());
        }
    }
}