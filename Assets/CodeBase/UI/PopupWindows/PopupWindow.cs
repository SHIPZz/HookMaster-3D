using System.Collections.Generic;
using CodeBase.Animations;
using CodeBase.Enums;
using CodeBase.Services.Camera;
using CodeBase.Services.DataService;
using CodeBase.Services.ShopItemData;
using CodeBase.Services.Window;
using CodeBase.SO.GameItem.CircleRoulette;
using CodeBase.SO.GameItem.MiningFarm;
using CodeBase.UI.Shop;
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
        
        private GameStaticDataService _gameStaticDataService;
        private string _name;
        private string _description;
        private Sprite _spriteIcon;
        private GameItemService _gameItemService;
        private GameItemType _gameItemType;
        private CameraService _cameraService;

        [Inject]
        private void Construct(GameStaticDataService gameStaticDataService,
            GameItemService gameItemService, CameraService cameraService)
        {
            _cameraService = cameraService;
            _gameItemService = gameItemService;
            _gameStaticDataService = gameStaticDataService;
        }

        public void Init(GameItemType gameItemType)
        {
            _gameItemType = gameItemType;
            
            switch (gameItemType)
            {
                case GameItemType.CircleRoulette:
                    var circleRouletteSO =  _gameStaticDataService.GetSO<CircleRouletteSO>();
                    _name = circleRouletteSO.Name;
                    _description = circleRouletteSO.Description;
                    _spriteIcon = circleRouletteSO.Icon;
                    break;

                case GameItemType.MiningFarm:
                    var miningFarmSo =  _gameStaticDataService.GetSO<MiningFarmSO>();
                    _name = miningFarmSo.Name;
                    _description = miningFarmSo.Description;
                    _spriteIcon = miningFarmSo.Icon;
                    break;
            }

            _nameText.text = _name;
            _descriptionText.text = _description;
            _icon.sprite = _spriteIcon;
        }
        
        public override void Open()
        {
            _canvasAnimator.FadeInCanvas();
        }

        public override void Close()
        {
            _gameItemService.Create(_gameItemType);
            _cameraService.MoveToLastTarget();
            base.Close();
        }
    }
}