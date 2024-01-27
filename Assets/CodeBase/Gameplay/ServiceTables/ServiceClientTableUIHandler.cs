using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ServiceTables
{
    public class ServiceClientTableUIHandler : MonoBehaviour
    {
        [SerializeField] private ServiceClientTable _serviceClientTable;

        private WindowService _windowService;

        [Inject]
        private void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

        private void OnEnable()
        {
            _serviceClientTable.PlayerApproached += EnableButton;
            _serviceClientTable.PlayerExited += DisableButton;
        }

        private void OnDisable()
        {
            _serviceClientTable.PlayerApproached -= EnableButton;
            _serviceClientTable.PlayerExited -= DisableButton;
        }

        private void EnableButton()
        {
            var hudWindow = _windowService.Get<HudWindow>();
            hudWindow.BuyClientManagerButton.Set(_serviceClientTable);
            hudWindow.BuyClientManagerButton.ToScale();
        }

        private void DisableButton()
        {
            var hudWindow = _windowService.Get<HudWindow>();
            hudWindow.BuyClientManagerButton.UnScale();
        }
    }
}