using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Services.Fire;
using CodeBase.Services.TriggerObserve;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Fire
{
    public class FireSystem : MonoBehaviour
    {
        [SerializeField] private float _putOutHitCount = 5f;
        [SerializeField] private List<ParticleSystem> _fires;
        [SerializeField] private ParticleSystem _fire;
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private TriggerObserver _changeMaterialTriggerObserver;
        [SerializeField] private float _disableTime = 15f;
        [SerializeField] private float _destroyTime = 3f;
        [SerializeField] private AudioSource _fireSound;

        private Dictionary<int, IBurnable> _burnableObjects = new();
        private FireService _fireService;
        private bool _destroyed;

        [Inject]
        private void Construct(FireService fireService)
        {
            _fireService = fireService;
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnSmoked;
            _changeMaterialTriggerObserver.TriggerEntered += SetObjectToChangeMaterial;
            _fire.Play();
            StartCoroutine(StartDisableTimer());
        }

        private IEnumerator StartDisableTimer()
        {
            while (_disableTime != 0)
            {
                yield return new WaitForSeconds(1f);
                _disableTime--;
            }

            BurnObjects();

            Stop();
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnSmoked;
            _changeMaterialTriggerObserver.TriggerEntered -= SetObjectToChangeMaterial;
        }

        private void SetObjectToChangeMaterial(Collider obj)
        {
            if (!obj.gameObject.TryGetComponent(out IBurnable burnable))
                return;

            if (_burnableObjects.ContainsKey(burnable.GetHashCode()))
                return;

            if (burnable.IsBurned)
                return;

            _burnableObjects[burnable.GetHashCode()] = burnable;
        }

        [Button]
        private void OnSmoked(Collider obj)
        {
            _putOutHitCount = Mathf.Clamp(_putOutHitCount--, 0, _putOutHitCount);

            if (_putOutHitCount == 0)
            {
                Stop();
                _burnableObjects.Values.ToList().ForEach(x => x.Recover());
            }
        }

        private void Stop()
        {
            _fire.Stop();
            _fireService.Reset();
            _destroyed = true;
            Destroy(gameObject, _destroyTime);
        }

        private void BurnObjects()
        {
            if (_destroyed)
                return;
            
            _fireSound.Play();
            
            foreach (var burnable in _burnableObjects.Values)
            {
                burnable.Burn();
            }
        }
    }
}