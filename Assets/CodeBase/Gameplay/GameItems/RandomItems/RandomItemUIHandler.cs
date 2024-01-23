using CodeBase.Services.DataService;
using CodeBase.Services.Window;
using CodeBase.SO.GameItem.RandomItems;
using CodeBase.UI.SuitCase;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.GameItems.RandomItems
{
    [RequireComponent(typeof(RandomItem))]
    public class RandomItemUIHandler : MonoBehaviour
    {
        private RandomItem _randomItem;
        private WindowService _windowService;
        private GameStaticDataService _gameStaticDataService;
        private bool _canOpen = true;

        [Inject]
        private void Construct(WindowService windowService, GameStaticDataService gameStaticDataService)
        {
            _gameStaticDataService = gameStaticDataService;
            _windowService = windowService;
        }
        
        private void Awake() => 
            _randomItem = GetComponent<RandomItem>();

        private void OnEnable()
        {
            _randomItem.PlayerApproached += OpenWindow;
            _randomItem.PlayerExited += SetCanOpen;
        }

        private void OnDisable()
        {
            _randomItem.PlayerApproached -= OpenWindow;
            _randomItem.PlayerExited -= SetCanOpen;
        }

        private void SetCanOpen()
        {
            _canOpen = true;
        }

        private void OpenWindow()
        {
            if(!_canOpen)
                return;
            
            RandomItemSO targetData = _gameStaticDataService.GetRandomItemSO(_randomItem.GameItemType);
            var targetWindow = _windowService.Get<RandomItemWindow>();
            targetWindow.Init(targetData.Name, $"{targetData.Profit}$", targetData.Icon, targetData.IconPosition);
            targetWindow.Open();
            _canOpen = false;
        }
    }
}