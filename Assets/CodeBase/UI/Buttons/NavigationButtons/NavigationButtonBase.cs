using CodeBase.Services.Camera;
using CodeBase.Services.Providers.Location;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Buttons.NavigationButtons
{
    public abstract class NavigationButtonBase : Button
    {
        protected CameraService CameraService;
        protected LocationProvider LocationProvider;

        [Inject]
        private void Construct(CameraService cameraService, LocationProvider locationProvider)
        {
            CameraService = cameraService;
            LocationProvider = locationProvider;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            onClick.AddListener(Navigate);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            onClick.RemoveListener(Navigate);
        }

        protected abstract void Navigate();
    }
}