using System;
using CodeBase.Constant;
using CodeBase.Gameplay.Clients;
using CodeBase.Services.Clients;
using CodeBase.Services.TriggerObserve;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ServiceTables
{
    public class ServiceTable : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Transform _servePoint;
        
        private ClientServeService _clientServeService;

        [Inject]
        private void Construct(ClientServeService clientServeService)
        {
            _clientServeService = clientServeService;
        }

        private void Start()
        {
            _clientServeService.SetTargetServePoint(_servePoint.position);
            _triggerObserver.TriggerEntered += OnCharacterEntered;
            _triggerObserver.TriggerExited += OnCharacterExited;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnCharacterEntered;
            _triggerObserver.TriggerExited -= OnCharacterExited;
        }

        private void OnCharacterEntered(Collider obj)
        {
            if (obj.gameObject.layer == LayerId.Player)
            {
                _clientServeService.SetPlayerApproached(true);
                return;
            }

            _clientServeService.SetClientApproached(true, obj.GetComponent<Client>());
        }

        private void OnCharacterExited(Collider obj)
        {
            if (obj.gameObject.layer == LayerId.Player)
            {
                _clientServeService.SetPlayerApproached(false);
                return;
            }

            _clientServeService.SetClientApproached(false, obj.GetComponent<Client>());
        }
    }
}