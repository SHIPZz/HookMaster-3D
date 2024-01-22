using UnityEngine;

namespace CodeBase.UI.Buttons.NavigationButtons
{
    public class ClientRoomNavigationButton : NavigationButtonBase
    {
        [SerializeField] private float _returnDuration = 2f;
        
        protected override void Navigate()
        {
            enabled = false;
            Transform clientRoom = LocationProvider.ClientServiceRoom;
            CameraService.MoveToTarget(clientRoom,_returnDuration);
        }
    }
}