using System;
using System.Globalization;
using Abu;
using CodeBase.Animations;
using CodeBase.Extensions;
using CodeBase.Services.WorldData;
using CodeBase.UI.Buttons;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Hud
{
    public class HudWindow : WindowBase
    {
        public OpenEmployeeWindowButton OpenEmployeeWindowButton;
        public TutorialFadeImage TutorialFadeImage;
        public Transform TutorialHandParent;
        
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private CanvasAnimator _canvasAnimator;
        
        private IWorldDataService _worldDataService;

        [Inject]
        private void Construct(IWorldDataService worldDataService) => 
            _worldDataService = worldDataService;

        private void Start()
        {
            DateTime currentTime = _worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime();
            string formattedTime = currentTime.ToString($"{currentTime.Day}/{currentTime.Month}/{currentTime.Year}", CultureInfo.InvariantCulture);
            _timeText.text = formattedTime;
        }
        
        public override void Open() => 
            _canvasAnimator.FadeInCanvas();
    }
}