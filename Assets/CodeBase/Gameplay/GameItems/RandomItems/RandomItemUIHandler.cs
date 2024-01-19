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

        [Inject]
        private void Construct(WindowService windowService, GameStaticDataService gameStaticDataService)
        {
            _gameStaticDataService = gameStaticDataService;
            _windowService = windowService;
        }
        
        private void Awake() => 
            _randomItem = GetComponent<RandomItem>();

        private void OnEnable() => 
            _randomItem.PlayerApproached += OpenWindow;

        private void OnDisable() => 
            _randomItem.PlayerApproached -= OpenWindow;

        private void OpenWindow()
        {
            RandomItemSO targetData = _gameStaticDataService.GetRandomItemSO(_randomItem.GameItemType);
            var targetWindow = _windowService.Get<RandomItemWindow>();
            targetWindow.Init(targetData.Name, $"{targetData.Profit}$", targetData.Icon, targetData.IconPosition);
            targetWindow.Open();
        }
    }
}