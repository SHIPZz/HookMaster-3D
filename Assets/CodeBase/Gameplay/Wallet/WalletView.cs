using CodeBase.Animations;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Services.Wallet;
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

        [Inject]
        private void Construct(WalletService walletService)
        {
            _walletService = walletService;
        }

        public void Start()
        {
            _walletService.MoneyChanged += SetMoney;

            _moneyText.SetText($"{_walletService.GetValue(ItemTypeId.Money)}$");
        }

        private void OnDisable()
        {
            _walletService.MoneyChanged -= SetMoney;
        }

        [Button]
        private void SetMoney(int money)
        {
            // _moneySound.Play();
            _moneyText.SetText($"{money}$");
            _moneyText.DoFadeOutColor(() => _moneyText.DoFadeInColor());
            _moneyText.DoScale(() => _moneyText.ResetScale());
        }
    }
}