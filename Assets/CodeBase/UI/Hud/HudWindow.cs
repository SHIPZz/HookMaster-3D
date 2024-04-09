using System;
using System.Globalization;
using CodeBase.Animations;
using CodeBase.Enums;
using CodeBase.Extensions;
using CodeBase.Gameplay.PurchaseableSystem;
using CodeBase.Gameplay.Tutorial;
using CodeBase.Services.PurchaseableItemServices;
using CodeBase.Services.WorldData;
using CodeBase.UI.Buttons;
using CodeBase.UI.Buttons.BuyButtons;
using CodeBase.UI.Buttons.NavigationButtons;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Hud
{
    public class HudWindow : WindowBase
    {
        [field: SerializeField] public OpenEmployeeWindowButton OpenEmployeeWindowButton { get; private set; }
        [field: SerializeField] public TutorialContainer TutorialContainer { get; private set; }
        [field: SerializeField] public BuyClientManagerButton BuyClientManagerButton { get; private set; }

        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private ClientRoomNavigationButton _clientRoomNavigationButton;

        private IWorldDataService _worldDataService;
        private PurchaseableItemService _purchaseableItemService;

        public ClientRoomNavigationButton ClientRoomNavigationButton => _clientRoomNavigationButton;

        [Inject]
        private void Construct(IWorldDataService worldDataService, PurchaseableItemService purchaseableItemService)
        {
            _purchaseableItemService = purchaseableItemService;
            _worldDataService = worldDataService;
        }

        private void Start()
        {
            DateTime currentTime = _worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime();
            string formattedTime = currentTime.ToString($"{currentTime.Day}/{currentTime.Month}/{currentTime.Year}",
                CultureInfo.InvariantCulture);
            _timeText.text = formattedTime;
            _purchaseableItemService.Purchased += TryDisableNavigationButton;
        }

        private void OnDestroy() =>
            _purchaseableItemService.Purchased -= TryDisableNavigationButton;

        public override void Open()
        {
            if (_purchaseableItemService.HasItem(GameItemType.ClientServiceDoor))
                _clientRoomNavigationButton.gameObject.SetActive(false);

            _canvasAnimator.FadeInCanvas();
        }

        private void TryDisableNavigationButton(PurchaseableItem purchaseableItem)
        {
            if (purchaseableItem.GameItemType == GameItemType.ClientServiceDoor)
                _clientRoomNavigationButton.gameObject.SetActive(false);
        }
    }
}