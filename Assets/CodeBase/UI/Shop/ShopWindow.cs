using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.UI;
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
        private FloatingTextService _floatingTextService;

        [Inject]
        private void Construct(WindowService windowService, WalletService walletService, 
            FloatingTextService floatingTextService)
        {
            _floatingTextService = floatingTextService;
            _walletService = walletService;
            _windowService = windowService;
        }
        
        public override void Open()
        {
            _shopTabViews.FirstOrDefault(x => x.ItemTypeId == ItemTypeId.Money)?.Init();
            
            _canvasAnimator.FadeInCanvas();
            _walletService.DiamondsChanged += SetDiamondCountText;
            _walletService.MoneyChanged += SetMoneyCountText;
            _walletService.TicketCountChanged += SetTicketCountText;
            SetDiamondCountText(_walletService.CurrentDiamonds);
            SetMoneyCountText(_walletService.CurrentMoney);
            SetTicketCountText(_walletService.CurrentTickets);
        }

        public override void Close()
        {
            _walletService.DiamondsChanged -= SetDiamondCountText;
            _walletService.MoneyChanged -= SetMoneyCountText;
            _walletService.TicketCountChanged -= SetTicketCountText;
            
            _canvasAnimator.FadeOutCanvas(() =>
            {
                _windowService.Open<HudWindow>();
                base.Close();
            });
        }

        private void SetDiamondCountText(int count) => 
            _diamondText.text = $"{count}";

        private void SetMoneyCountText(int count) => 
            _moneyText.text = $"{count}$";

        private void SetTicketCountText(int count) => 
            _ticketText.text = $"{count}";
    }
}