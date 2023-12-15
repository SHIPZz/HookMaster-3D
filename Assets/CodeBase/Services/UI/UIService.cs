using CodeBase.Constant;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Location;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.UI
{
    public class UIService : IInitializable
    {
        private readonly UIFactory _uiFactory;
        private readonly UIProvider _uiProvider;
        private readonly LocationProvider _locationProvider;

        public UIService(UIFactory uiFactory, UIProvider uiProvider, LocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
            _uiFactory = uiFactory;
            _uiProvider = uiProvider;
        }

        public void Initialize()
        {
           var floatingTextCanvas = _uiFactory.CreateElement<Canvas>(AssetPath.FloatingTextCanvas, _locationProvider.UIParent);
           _uiProvider.FloatingTextCanvas = floatingTextCanvas;
        }
    }
}