using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Wallet
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyText;
        private WalletService _walletService;

        [Inject]
        private void Construct(WalletService walletService) => 
            _walletService = walletService;

        public void OnEnable() => 
            _walletService.MoneyChanged += SetMoney;

        private void OnDisable() => 
            _walletService.MoneyChanged -= SetMoney;

        private void SetMoney(int money) => 
            _moneyText.text = money.ToString();
    }
}