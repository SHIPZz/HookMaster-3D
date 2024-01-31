using CodeBase.Services.Window;
using CodeBase.SO.InfoItems;
using CodeBase.UI.Info;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Buttons.NavigationButtons
{
    public class ClientRoomNavigationButton : NavigationButtonBase
    {
        [SerializeField] private float _returnDuration = 2f;

        [Inject] private WindowService _windowService;

        protected override void Navigate()
        {
            enabled = false;
            Transform clientRoom = LocationProvider.ClientServiceRoom;
            var infoWindow = _windowService.Get<InfoWindow>();
            infoWindow.Init(InfoItemTypeId.ClientServeRoom, _windowService);
            infoWindow.Open();
            CameraService.MoveToTarget(clientRoom, _returnDuration);
        }
    }
}