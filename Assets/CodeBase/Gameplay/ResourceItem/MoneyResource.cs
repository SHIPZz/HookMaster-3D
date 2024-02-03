using CodeBase.Data;
using CodeBase.Gameplay.SoundPlayer;
using CodeBase.Services.Wallet;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ResourceItem
{
    public class MoneyResource : Resource
    {
        [field: SerializeField] public int Value { get; private set; }
        [SerializeField] private SoundPlayerSystem _soundPlayerSystem;
        private WalletService _walletService;

        [Inject]
        private void Construct(WalletService walletService)
        {
            _walletService = walletService;
        }

        public override void Collect(Transform parent)
        {
            base.Collect(parent);
            _soundPlayerSystem.PlayActiveSound();
            _walletService.Set(ItemTypeId.Money, Value);
        }
    }
}