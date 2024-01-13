using System;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.PurchaseableItemServices;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Gameplay.PurchaseableSystem
{
    public class PurchaseableItemUIHandler : MonoBehaviour
    {
        [SerializeField] private PurchaseableItem _purchaseableItem;
        [SerializeField] private Button _accessButton;
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private float _additionalPosition = 5f;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private TextMeshProUGUI _priceText;

        private WalletService _walletService;
        private FloatingButtonService _floatingButtonService;
        private PurchaseableItemService _purchaseableItemService;

        [Inject]
        private void Construct(WalletService walletService, FloatingButtonService floatingButtonService,
            PurchaseableItemService purchaseableItemService)
        {
            _purchaseableItemService = purchaseableItemService;
            _floatingButtonService = floatingButtonService;
            _walletService = walletService;
        }

        private void Start()
        {
            _triggerObserver.TriggerEntered += OnPlayerEntered;
            _triggerObserver.TriggerExited += OnPlayerExited;
            _accessButton.onClick.AddListener(OnAccessClicked);
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnPlayerEntered;
            _triggerObserver.TriggerExited -= OnPlayerExited;
            _accessButton.onClick.RemoveListener(OnAccessClicked);
        }

        private void OnPlayerExited(Collider obj)
        {
            if (_accessButton.gameObject.activeSelf)
                _floatingButtonService
                    .ShowFloatingButton(-_additionalPosition,
                        _duration, Quaternion.identity, false, _accessButton,
                        () => _accessButton.gameObject.SetActive(false));

            if (_purchaseableItem.IsAсcessible)
                return;

            if (!_walletService.HasEnough(ItemTypeId.Money, _purchaseableItem.Price))
                return;
        }

        private void OnPlayerEntered(Collider obj)
        {
            if (_purchaseableItem.IsAсcessible)
                return;

            if (!_walletService.HasEnough(ItemTypeId.Money, _purchaseableItem.Price))
                return;

            _priceText.text = $"{_purchaseableItem.Price}$";
            _floatingButtonService
                .ShowFloatingButton(_additionalPosition, _duration, Quaternion.identity, true, _accessButton);
        }

        private void OnAccessClicked()
        {
            _accessButton.gameObject.SetActive(false);
            _walletService.Set(ItemTypeId.Money, -_purchaseableItem.Price);
            _purchaseableItem.IsAсcessible = true;
            _purchaseableItemService.SaveToData(_purchaseableItem);
        }
    }
}