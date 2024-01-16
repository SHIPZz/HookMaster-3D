using System;
using CodeBase.Animations;
using CodeBase.Constant;
using CodeBase.Gameplay.Clients;
using CodeBase.Services.Clients;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.UI;
using CodeBase.UI.FloatingText;
using CodeBase.UI.TimeSlider;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ServiceTables
{
    public class ServiceClientTable : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Transform _servePoint;
        [SerializeField] private TimeSliderView _timeSliderView;

        private ClientServeService _clientServeService;
        private FloatingTextService _floatingTextService;

        [Inject]
        private void Construct(ClientServeService clientServeService, FloatingTextService floatingTextService)
        {
            _floatingTextService = floatingTextService;
            _clientServeService = clientServeService;
        }

        private void Start()
        {
            _clientServeService.SetTargetServePoint(_servePoint);
            _triggerObserver.TriggerEntered += OnCharacterEntered;
            _triggerObserver.TriggerExited += OnCharacterExited;
            _clientServeService.Started += OnServingStarted;
            _clientServeService.Finished += OnServingFinished;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnCharacterEntered;
            _triggerObserver.TriggerExited -= OnCharacterExited;
            _clientServeService.Started -= OnServingStarted;
            _clientServeService.Finished -= OnServingFinished;
        }

        private void OnServingStarted(float serveTime) =>
            _timeSliderView.FillToMaxValue(serveTime);

        private void OnServingFinished(int reward)
        {
            _timeSliderView.Disable();
            _floatingTextService.ShowFloatingText(FloatingTextType.MoneyProfit, transform, transform.position, $"{reward}$", 2f,0.5f);
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
                _timeSliderView.Stop();

                _clientServeService.SetPlayerApproached(false);
                return;
            }

            _clientServeService.SetClientApproached(false, obj.GetComponent<Client>());
        }
    }
}