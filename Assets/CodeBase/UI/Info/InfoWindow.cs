using CodeBase.Animations;
using CodeBase.Gameplay.SoundPlayer;
using CodeBase.Services.DataService;
using CodeBase.Services.Pause;
using CodeBase.SO.InfoItems;
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

        [Inject] private IPauseService _pauseService;

        public void Init(InfoItemTypeId infoItemTypeId)
        {
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

        public override void Close()
        {
            _pauseService.Run();
            base.Close();
        }
    }
}