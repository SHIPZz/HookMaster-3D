using CodeBase.Animations;
using CodeBase.Enums;
using CodeBase.Gameplay.SoundPlayer;
using CodeBase.Services.DataService;
using CodeBase.Services.Window;
using CodeBase.SO.InfoItems;
using CodeBase.UI.Hud;
using CodeBase.UI.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Info
{
    public class InfoWindow : WindowBase
    {
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _icon;
        [SerializeField] private SoundPlayerSystem _soundPlayerSystem;

        [Inject] private GameStaticDataService _gameStaticDataService;
        private string _name;
        private string _description;
        private Sprite _spriteIcon;
        private WindowService _windowService;

        public void Init(InfoItemTypeId infoItemTypeId, WindowService windowService)
        {
            _windowService = windowService;
            InfoItemSO data = _gameStaticDataService.Get(infoItemTypeId);
            _nameText.text = data.Name;
            _descriptionText.text = data.Description;
            _icon.sprite = data.Icon;
        }

        public override void Open()
        {
            _soundPlayerSystem.PlayActiveSound();
            _canvasAnimator.FadeInCanvas();
        }
    }
}