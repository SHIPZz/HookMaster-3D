using System;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using UnityEngine;

namespace CodeBase.Gameplay.Tutorial
{
    public class ApproachToPaperTableStep : TutorialStep, IDisposable
    {
        private readonly LocationProvider _locationProvider;

        public ApproachToPaperTableStep(UIFactory uiFactory,
            WindowService windowService,
            IWorldDataService worldDataService, LocationProvider locationProvider)
            : base(uiFactory, windowService, worldDataService)
        {
            _locationProvider = locationProvider;
        }

        public override void OnStart()
        {
            if (IsCompleted())
                return;

            Debug.Log(_locationProvider.PaperCreatorTable.Pointer.name);
            _locationProvider.PaperCreatorTable.Pointer.gameObject.SetActive(true);
            _locationProvider.PaperCreatorTable.PlayerApproached += OnFinished;
        }

        public override void OnFinished()
        {
            _locationProvider.PaperCreatorTable.Pointer.SetActive(false);
            _locationProvider.PaperCreatorTable.PlayerApproached -= OnFinished;
            SetCompleteToData(true);
        }

        public void Dispose()
        {
            _locationProvider.PaperCreatorTable.PlayerApproached -= OnFinished;
        }
    }
}