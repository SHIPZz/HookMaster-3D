using System.Collections.Generic;
using CodeBase.Animations;
using CodeBase.Gameplay.SoundPlayer;
using CodeBase.Services.DataService;
using CodeBase.Services.Pause;
using CodeBase.SO.InfoItems;
using I2.Loc;
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
        [SerializeField] private List<Localize> _localizes;

        [Inject] private GameStaticDataService _gameStaticDataService;
        [Inject] private IPauseService _pauseService;
        
        private string _name;
        private string _description;
        private Sprite _spriteIcon;


        public void Init(InfoItemTypeId infoItemTypeId)
        {
            InfoItemSO data = _gameStaticDataService.Get(infoItemTypeId);
            _nameText.text = data.Name;
            _descriptionText.text = data.Description;
            _icon.sprite = data.Icon;
            _localizes.ForEach(x=> x.OnLocalize(true));
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