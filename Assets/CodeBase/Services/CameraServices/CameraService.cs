using System;
using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.ShopItemData;
using CodeBase.Services.UI;
using CodeBase.Services.Window;
using CodeBase.UI;
using CodeBase.UI.Hud;
using CodeBase.UI.PopupWindows;
using CodeBase.UI.Shop;
using UnityEngine;

namespace CodeBase.Services.Camera
{
    public class CameraService : IDisposable
    {
        private readonly CameraProvider _cameraProvider;
        private readonly GameItemService _gameItemService;
        private readonly WindowService _windowService;
        private readonly UIService _uiService;
        private List<GameItemType> _shownObjects = new();
        private List<GameItemType> _targetCameraPans = new()
        {
            GameItemType.CircleRoulette,
            GameItemType.MiningFarm,
        };
        
        private GameItemAbstract _target;
        private ShopWindow _shopWindow;

        public CameraService(CameraProvider cameraProvider,
            GameItemService gameItemService,
            WindowService windowService, UIService uiService)
        {
            _uiService = uiService;
            _windowService = windowService;
            _gameItemService = gameItemService;
            _cameraProvider = cameraProvider;
        }

        public void Init()
        {
            _gameItemService.Created += SetTarget;
        }

        public void Dispose()
        {
            _gameItemService.Created -= SetTarget;
        }

        private void SetTarget(GameItemAbstract gameItem)
        {
            if(!_targetCameraPans.Contains(gameItem.GameItemType))
                return;

            _target = gameItem;
        }

        public void MoveToLastTarget()
        {
            if(_shownObjects.Contains(_target.GameItemType))
                return;
            
            _uiService.SetActiveUI<HudWindow>(false);
            _cameraProvider.CameraFollower.Block(true);
            _cameraProvider.CameraFollower.MoveTo(_target.transform, () => _uiService.SetActiveUI(true));
            _shownObjects.Add(_target.GameItemType);
        }
    }
}