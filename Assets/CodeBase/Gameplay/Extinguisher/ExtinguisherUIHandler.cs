using System;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.Window;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Extinguisher
{
    public class ExtinguisherUIHandler : MonoBehaviour
    {
        [SerializeField] private ExtinguisherSystem _extinguisher;
        [SerializeField] private IKObjectSystem _ikObjectSystem;
        private WindowService _windowService;

        [Inject]
        private void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

        private void OnEnable()
        {
            _ikObjectSystem.PlayerTaken += OnTaken;
        }

        private void OnDisable()
        {
            _ikObjectSystem.PlayerTaken -= OnTaken;
        }

        private void OnTaken()
        {
            var putOutWindow = _windowService.Get<PutOutWindow>();
            putOutWindow.Init(_extinguisher);
            putOutWindow.Open();
        }
    }
}