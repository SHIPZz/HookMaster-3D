using System;
using CodeBase.Constant;
using CodeBase.Gameplay.Clients;
using CodeBase.Gameplay.ObjectCreatorSystem;
using CodeBase.Services.Clients;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.UI;
using CodeBase.Services.WorldData;
using CodeBase.UI.TimeSlider;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ServiceTables
{
    public class ServiceClientTable : MonoBehaviour
    {
        public string Id;

        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private TriggerObserver _clientObserver;
        [SerializeField] private Transform _servePoint;
        [SerializeField] private TimeSliderView _timeSliderView;
        [SerializeField] private ManagerChair _manager;
        [SerializeField] private ResourceCreator _resourceCreator;
        [SerializeField] private float _managerServeDelay = 2.5f;

        private ClientServeService _clientServeService;
        private IWorldDataService _worldDataService;
        private bool _managerPurchased;

        public event Action PlayerApproached;
        public event Action PlayerExited;

        [Inject]
        private void Construct(ClientServeService clientServeService, IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
            _clientServeService = clientServeService;
        }

        private void Start()
        {
            _clientServeService.SetTargetServePoint(_servePoint);

            if (_worldDataService.WorldData.PlayerData.PurchasedManagers.ContainsKey(Id) &&
                _worldDataService.WorldData.PlayerData.PurchasedManagers[Id])
            {
                _managerPurchased = true;
                _manager.Enable();
                _clientServeService.SetPlayerApproached(true);
                _clientServeService.SetServeDelay(_managerServeDelay);
            }

            SubscribeObservers();

            _clientServeService.Started += OnServingStarted;
            _clientServeService.Finished += OnServingFinished;
        }

        private void OnDisable()
        {
            UnsubscribeObservers();
            _clientServeService.Started -= OnServingStarted;
            _clientServeService.Finished -= OnServingFinished;
        }

        public void EnableManagerChair()
        {
            _manager.Enable();
            _clientServeService.SetPlayerApproached(true);
            _managerPurchased = true;
            _clientServeService.SetServeDelay(_managerServeDelay);
            _worldDataService.WorldData.PlayerData.PurchasedManagers[Id] = true;
            PlayerExited?.Invoke();
        }

        private void UnsubscribeObservers()
        {
            _triggerObserver.TriggerEntered -= OnCharacterEntered;
            _triggerObserver.TriggerExited -= OnCharacterExited;
            _clientObserver.TriggerEntered -= OnCharacterEntered;
            _clientObserver.TriggerExited -= OnCharacterExited;
        }

        private void SubscribeObservers()
        {
            _triggerObserver.TriggerEntered += OnCharacterEntered;
            _triggerObserver.TriggerExited += OnCharacterExited;
            _clientObserver.TriggerEntered += OnCharacterEntered;
            _clientObserver.TriggerExited += OnCharacterExited;
        }

        private void OnServingStarted(float serveTime) =>
            _timeSliderView.FillToMaxValue(serveTime);

        private void OnServingFinished(int reward)
        {
            _timeSliderView.Disable();
            _resourceCreator.Create();
        }

        private void OnCharacterEntered(Collider character)
        {
            if (character.gameObject.layer == LayerId.Player)
            {
                if (_managerPurchased)
                    return;

                _clientServeService.SetPlayerApproached(true);
                PlayerApproached?.Invoke();
                return;
            }

            _clientServeService.SetClientApproached(true, character.GetComponent<Client>());
        }

        private void OnCharacterExited(Collider obj)
        {
            if (obj.gameObject.layer == LayerId.Player)
            {
                if (_managerPurchased)
                    return;

                _timeSliderView.Stop();
                _clientServeService.Stop();
                PlayerExited?.Invoke();
                _clientServeService.SetPlayerApproached(false);
                return;
            }

            _clientServeService.SetClientApproached(false, obj.GetComponent<Client>());
        }

        [Button]
        private void CreateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}