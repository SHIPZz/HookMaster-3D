using System;
using CodeBase.Enums;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Wallet
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private float _targetScale = 2f;
        [SerializeField] private float _increaseScaleDuration = 0.5f;
        [SerializeField] private float _decreaseScaleDuration = 0.1f;
        [SerializeField] private float _defaultScale = 1f;

        private WalletService _walletService;
        private Color _moneyColor;
        private RectTransform _moneyTextRectTransform;
        private Tween _tween;

        [Inject]
        private void Construct(WalletService walletService, [Inject(Id = ColorTypeId.Money)] Color moneyColor)
        {
            _moneyColor = moneyColor;
            _walletService = walletService;
        }

        private void Awake() => 
            _moneyTextRectTransform = _moneyText.GetComponent<RectTransform>();

        public void OnEnable()
        {
            _walletService.MoneyChanged += SetMoney;
            _moneyText.color = _moneyColor;
        }

        private void OnDisable() =>
            _walletService.MoneyChanged -= SetMoney;

        [Button]
        private void SetMoney(int money)
        {
            _moneyText.text = money.ToString();
            _tween?.Kill(true);
            _tween = _moneyTextRectTransform.DOScale(_targetScale, _increaseScaleDuration)
                .OnComplete(() => _moneyTextRectTransform.DOScale(_defaultScale, _decreaseScaleDuration));
        }
    }
}