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
        [SerializeField] private TextAnimView _ticketCountText;
        [SerializeField] private TextAnimView _diamondCountText;
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
            
            _moneyText.SetText($"{_walletService.GetValue(ItemTypeId.Money)}$");
            _ticketCountText.SetText(_walletService.GetValue(ItemTypeId.Ticket));
            _diamondCountText.SetText(_walletService.GetValue(ItemTypeId.Diamond));
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
            // _moneySound.Play();
            _moneyText.SetText($"{money}$");
            _moneyText.DoFadeOutColor(() => _moneyText.DoFadeInColor());
            _moneyText.DoScale(() => _moneyText.ResetScale());
        }

        private void SetTickets(int amount)
        {
            _ticketCountText.SetText(amount);
            _ticketCountText.DoFadeOutColor(() => _ticketCountText.DoFadeInColor());
            _ticketCountText.DoScale(() => _ticketCountText.ResetScale());
        }


        private void SetDiamonds(int amount)
        {
            _diamondCountText.SetText(amount);
            _diamondCountText.DoFadeInColor(() => _diamondCountText.DoFadeOutColor());
            _diamondCountText.DoScale(() => _diamondCountText.ResetScale());
        }
    }
}