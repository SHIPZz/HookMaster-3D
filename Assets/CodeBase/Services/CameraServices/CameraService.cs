﻿using System;
using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.ShopItemData;
using CodeBase.Services.Window;
using CodeBase.UI;
using CodeBase.UI.Shop;
using UnityEngine;

namespace CodeBase.Services.Camera
{
    public class CameraService : IDisposable
    {
        private readonly CameraProvider _cameraProvider;
        private readonly GameItemService _gameItemService;
        private readonly WindowService _windowService;
        private List<GameItemType> _shownObjects = new();
        private List<GameItemType> _targetCameraPans = new()
        {
            GameItemType.CircleRoulette,
            GameItemType.MiningFarm,
        };
        
        private GameItemAbstract _target;
        private ShopWindow _shopWindow;

        public CameraService(CameraProvider cameraProvider, GameItemService gameItemService, WindowService windowService)
        {
            _windowService = windowService;
            _gameItemService = gameItemService;
            _cameraProvider = cameraProvider;
        }

        public void Init()
        {
            _gameItemService.Created += TryCameraPan;
            _windowService.Opened += OnWindowOpened;
        }

        public void Dispose()
        {
            _gameItemService.Created -= TryCameraPan;
            _windowService.Opened -= OnWindowOpened;

            if (_shopWindow != null)
                _shopWindow.Closed -= MoveToLastTarget;
        }

        private void OnWindowOpened(WindowBase window)
        {
            if(window.GetType() != typeof(ShopWindow))
                return;

            _shopWindow = _windowService.Get<ShopWindow>();
            _shopWindow.Closed += MoveToLastTarget;
        }

        private void TryCameraPan(GameItemAbstract gameItem)
        {
            if(!_targetCameraPans.Contains(gameItem.GameItemType))
                return;

            _target = gameItem;
        }

        private void MoveToLastTarget()
        {
            if(_shownObjects.Contains(_target.GameItemType))
                return;
            
            _cameraProvider.CameraFollower.Block(true);
            _cameraProvider.CameraFollower.MoveTo(_target.transform);
            _shownObjects.Add(_target.GameItemType);
        }
    }
}