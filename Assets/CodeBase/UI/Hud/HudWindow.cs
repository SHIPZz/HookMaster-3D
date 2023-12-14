using System;
using System.Globalization;
using CodeBase.Extensions;
using CodeBase.Services.WorldData;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Hud
{
    public class HudWindow : WindowBase
    {
        [SerializeField] private TMP_Text _timeText;
        private IWorldDataService _worldDataService;

        [Inject]
        private void Construct(IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
        }

        private void OnEnable()
        {
            DateTime currentTime = _worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime();
            string formattedTime = currentTime.ToString($"{currentTime.Day}/{currentTime.Month}/{currentTime.Year}", CultureInfo.InvariantCulture);
            _timeText.text = formattedTime;
        }
        
        public override void Open()
        {
            
        }
    }
}