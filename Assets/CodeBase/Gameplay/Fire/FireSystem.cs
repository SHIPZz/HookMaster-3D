using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.Fire
{
    public class FireSystem : MonoBehaviour
    {
        [SerializeField] private float _putOutHitCount = 5f;
        [SerializeField] private List<ParticleSystem> _fires;
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private TriggerObserver _changeMaterialTriggerObserver;

        private Dictionary<int, IBurnable> _burnableObjects = new();

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnSmoked;
            _changeMaterialTriggerObserver.TriggerEntered += SetObjectToChangeMaterial;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnSmoked;
            _changeMaterialTriggerObserver.TriggerEntered -= SetObjectToChangeMaterial;
        }

        private void SetObjectToChangeMaterial(Collider obj)
        {
            if(!obj.gameObject.TryGetComponent(out IBurnable burnable))
                return;
            
            if(_burnableObjects.ContainsKey(burnable.GetHashCode()))
                return;

            if(burnable.IsBurned)
                return;
            
            _burnableObjects[burnable.GetHashCode()] = burnable;
            burnable.Burn();
        }

        private void OnSmoked(Collider obj)
        {
            _putOutHitCount = Mathf.Clamp(_putOutHitCount--, 0, _putOutHitCount);

            if (_putOutHitCount == 0)
            {
                _fires.ForEach(x => x.Stop());
                _burnableObjects.Values.ToList().ForEach(x=>x.Recover());
            }
        }
    }
}