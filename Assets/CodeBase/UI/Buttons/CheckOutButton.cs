using System;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Wallet;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Buttons
{
    public class CheckOutButton : Button
    {
        [SerializeField] protected int Value;
        [SerializeField] protected ItemTypeId ItemTypeId;

        public event Action Successful;

        protected WalletService WalletService;
        private Button _button;

        [Inject]
        private void Construct(WalletService walletService)
        {
            WalletService = walletService;
        }

        protected override void Awake()
        {
            onClick.AddListener(OnClicked);
        }

        protected override void OnDisable()
        {
            onClick.RemoveListener(OnClicked);
        }

        protected virtual void OnClicked()
        {
            if (!WalletService.HasEnough(ItemTypeId, Value))
                return;
            
            WalletService.Set(ItemTypeId, -Value);
            Successful?.Invoke();
        }
    }
}