using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Shop
{
    public class ShopWindow : WindowBase
    {
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private TMP_Text _diamondText;
        [SerializeField] private TMP_Text _ticketText;
        [SerializeField] private List<ShopTabView> _shopTabViews;

        private WindowService _windowService;
        private WalletService _walletService;

        [Inject]
        private void Construct(WindowService windowService, WalletService walletService)
        {
            _walletService = walletService;
            _windowService = windowService;
        }
        
        public override void Open()
        {
            _shopTabViews.FirstOrDefault(x => x.ItemTypeId == ItemTypeId.Money)?.Init();
            
            _canvasAnimator.FadeInCanvas();
            _moneyText.text = $"{_walletService.CurrentMoney}";
            _diamondText.text = $"{_walletService.CurrentDiamonds}";
            _ticketText.text = $"{_walletService.CurrentTickets}";
        }

        public override void Close()
        {
            _canvasAnimator.FadeOutCanvas(() =>
            {
                _windowService.Open<HudWindow>();
                base.Close();
            });
        }
    }
}