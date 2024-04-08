using CodeBase.Animations;
using CodeBase.Services.CameraServices;
using CodeBase.Services.Window;
using CodeBase.SO.InfoItems;
using CodeBase.UI.Info;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Buttons.NavigationButtons
{
    public class ClientRoomNavigationButton : NavigationButtonBase
    {
        [SerializeField] private TransformScaleAnim _transformScaleAnim;

        [Inject] private WindowService _windowService;

        private FocusInfo _focusInfoTarget;
        private InfoWindow _infoWindow;
        private bool _canReleaseCamera;

        protected override void Start()
        {
            Transform clientRoom = LocationProvider.ClientServiceRoom;
            _focusInfoTarget = new FocusInfo { Target = clientRoom, CanReleaseAsync = CanReleaseAsync };
            CameraFocus.TargetReached += CameraReachedTargetHandler;
            CameraFocus.Moved += Hide;
        }

        protected override void OnDestroy()
        {
            CameraFocus.TargetReached -= CameraReachedTargetHandler;
            CameraFocus.Moved -= Hide;
            base.OnDestroy();
        }

        protected override void Navigate()
        {
            enabled = false;
            _transformScaleAnim.UnScale();
            CameraFocus.Moved -= Hide;

            CameraFocus.AddFocusTarget(_focusInfoTarget);
        }

        private async UniTask<bool> CanReleaseAsync()
        {
            await UniTask.WaitUntil(() => _canReleaseCamera);
            return _canReleaseCamera;
        }

        private async void CameraReachedTargetHandler(FocusInfo focusInfo)
        {
            if (focusInfo != _focusInfoTarget)
                return;

            _infoWindow = _windowService.Get<InfoWindow>();
            _infoWindow.Init(InfoItemTypeId.ClientServeRoom);
            _windowService.OpenCurrentWindow();
            _canReleaseCamera = true;

            await UniTask.WaitUntil(() => CameraFocus.HasFollow == false);
            ShowUp();
        }

        private void ShowUp()
        {
            if(gameObject != null && enabled)
                return;
            
            enabled = true;
            _transformScaleAnim.ToScale();
        }

        private async void Hide()
        {
            if(gameObject != null && !enabled)
                return;
            
            enabled = false;
            _transformScaleAnim.UnScale();
            await UniTask.WaitUntil(() => CameraFocus.HasFollow == false);
            ShowUp();
        }
    }
}