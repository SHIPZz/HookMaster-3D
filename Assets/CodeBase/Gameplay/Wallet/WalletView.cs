using CodeBase.Animations;
using CodeBase.Data;
using CodeBase.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Wallet
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TextAnimView _moneyText;
        [SerializeField] private AudioSource _moneySound;

        private WalletService _walletService;
        private Color _moneyColor;

        [Inject]
        private void Construct(WalletService walletService, [Inject(Id = ColorTypeId.Money)] Color moneyColor)
        {
            _moneyColor = moneyColor;
            _walletService = walletService;
        }

        public void Start()
        {
            _walletService.MoneyChanged += SetMoney;
            _walletService.TicketCountChanged += SetTickets;
            _walletService.DiamondsChanged += SetDiamonds;
            
            _moneyText.SetText(_walletService.GetValue(ItemTypeId.Money));
            SetTickets(_walletService.GetValue(ItemTypeId.Ticket));
            SetDiamonds(_walletService.GetValue(ItemTypeId.Diamond));
        }

        private void OnDisable()
        {
            _walletService.MoneyChanged -= SetMoney;
            _walletService.TicketCountChanged -= SetTickets;
            _walletService.DiamondsChanged -= SetDiamonds;
        }

        [Button]
        private void SetMoney(int money)
        {
            _moneySound.Play();
            _moneyText.SetText(money);
            _moneyText.DoFadeInColor(Color.white, () => _moneyText.DoFadeOutColor(_moneyColor));
            _moneyText.DoScale(() => _moneyText.ResetScale());
        }

        private void SetTickets(int amount)
        {
            
        }


        private void SetDiamonds(int amount)
        {
            
        }
    }
}