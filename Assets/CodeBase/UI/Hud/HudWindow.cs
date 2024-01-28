using System;
using System.Globalization;
using Abu;
using CodeBase.Animations;
using CodeBase.Enums;
using CodeBase.Extensions;
using CodeBase.Gameplay.PurchaseableSystem;
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
        public OpenEmployeeWindowButton OpenEmployeeWindowButton;
        public TutorialFadeImage TutorialFadeImage;
        public Transform TutorialHandParent;
        public BuyClientManagerButton BuyClientManagerButton;
        public Transform MoneyPosition;

        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private ClientRoomNavigationButton _clientRoomNavigationButton;

        private IWorldDataService _worldDataService;
        private PurchaseableItemService _purchaseableItemService;

        [Inject]
        private void Construct(IWorldDataService worldDataService, PurchaseableItemService purchaseableItemService)
        {
            _purchaseableItemService = purchaseableItemService;
            _worldDataService = worldDataService;
        }

        private void Start()
        {
            DateTime currentTime = _worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime();
            string formattedTime = currentTime.ToString($"{currentTime.Day}/{currentTime.Month}/{currentTime.Year}", CultureInfo.InvariantCulture);
            _timeText.text = formattedTime;
            _purchaseableItemService.Purchased += TryDisableNavigationButton;
        }

        private void OnDisable()
        {
            _purchaseableItemService.Purchased -= TryDisableNavigationButton;
        }

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