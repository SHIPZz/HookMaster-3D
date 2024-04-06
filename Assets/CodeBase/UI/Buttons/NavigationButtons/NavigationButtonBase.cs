using CodeBase.Services.CameraServices;
using CodeBase.Services.Providers.Location;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Buttons.NavigationButtons
{
    public abstract class NavigationButtonBase : Button
    {
        protected CameraFocus CameraFocus;
        protected LocationProvider LocationProvider;

        [Inject]
        private void Construct(LocationProvider locationProvider, CameraFocus cameraFocus)
        {
            CameraFocus = cameraFocus;
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