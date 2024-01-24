using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Animations;
using CodeBase.Data;
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
        [SerializeField] private RectTransformScaleAnim _rectTransformScale;

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
            _rectTransformScale.ToScale();
            _canvasAnimator.FadeInCanvas();

            _walletService.Set(ItemTypeId.Money, 30000);
            _walletService.Set(ItemTypeId.Diamond, 30000);
            _walletService.Set(ItemTypeId.Ticket, 30000);
            _walletService.DiamondsChanged += SetDiamondCountText;
            _walletService.MoneyChanged += SetMoneyCountText;
            _walletService.TicketCountChanged += SetTicketCountText;
            
            SetDiamondCountText(_walletService.GetValue(ItemTypeId.Diamond));
            SetMoneyCountText(_walletService.GetValue(ItemTypeId.Money));
            SetTicketCountText(_walletService.GetValue(ItemTypeId.Ticket));
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