using System;
using System.Collections.Generic;
using CodeBase.Animations;
using CodeBase.Enums;
using CodeBase.Gameplay;
using CodeBase.Gameplay.SoundPlayer;
using CodeBase.Services.CameraServices;
using CodeBase.Services.DataService;
using CodeBase.Services.GameItemServices;
using CodeBase.Services.Window;
using CodeBase.SO;
using CodeBase.SO.GameItem.CircleRoulette;
using CodeBase.SO.GameItem.Mining;
using CodeBase.UI.Hud;
using CodeBase.UI.Shop;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.PopupWindows
{
    public class PopupWindow : WindowBase
    {
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _icon;
        [SerializeField] private SoundPlayerSystem _soundPlayerSystem;
        [SerializeField] private AppearanceEffect _appearanceEffect;

        private GameStaticDataService _gameStaticDataService;
        private string _name;
        private string _description;
        private Sprite _spriteIcon;
        private GameItemService _gameItemService;
        private GameItemType _gameItemType;
        private CameraService _cameraService;
        private WindowService _windowService;

        [Inject]
        private void Construct(GameStaticDataService gameStaticDataService,
            GameItemService gameItemService, CameraService cameraService)
        {
            _cameraService = cameraService;
            _gameItemService = gameItemService;
            _gameStaticDataService = gameStaticDataService;
        }

        public void Init(GameItemType gameItemType, WindowService windowService)
        {
            _windowService = windowService;
            windowService.Close<ShopWindow>();
            _gameItemType = gameItemType;

            PopupAbstractSO targetData = _gameStaticDataService.GetSO(gameItemType);
            _name = targetData.Name;
            _description = targetData.Description;
            _spriteIcon = targetData.Sprite;

            _nameText.text = _name;
            _descriptionText.text = _description;
            _icon.sprite = _spriteIcon;
        }

        public override void Open()
        {
            _canvasAnimator.FadeInCanvas(OnOpened);
        }

        private void OnOpened()
        {
            _windowService.Close<HudWindow>();
            _soundPlayerSystem.PlayActiveSound();
            _appearanceEffect.PlayTargetEffects();
        }

        public override void Close()
        {
            _gameItemService.Create(_gameItemType);
            _cameraService.MoveToLastTarget();
            base.Close();
        }
    }
}