using System;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class CheckOutButton : MonoBehaviour
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

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            if (!WalletService.HasEnough(ItemTypeId, Value))
                return;
            
            WalletService.Set(ItemTypeId, -Value);
            Successful?.Invoke();
        }
    }
}