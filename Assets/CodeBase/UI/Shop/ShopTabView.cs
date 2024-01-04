using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Asset;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Shop
{
    public class ShopTabView : MonoBehaviour
    {
        [field: SerializeField] public ItemTypeId ItemTypeId { get; private set; }
        [SerializeField] private Transform _parent;

        private UIFactory _uiFactory;
        private IAssetProvider _assetProvider;
        private WalletService _walletService;

        [Inject]
        private void Construct(UIFactory uiFactory, IAssetProvider assetProvider, WalletService walletService)
        {
            _walletService = walletService;
            _assetProvider = assetProvider;
            _uiFactory = uiFactory;
        }

        public void Init()
        {
            IEnumerable<ShopItemView> shopItemViews = _assetProvider
                .GetAll<ShopItemView>(AssetPath.ShopItemViews)
                .Where(x => x.ItemTypeId == ItemTypeId);

            foreach (ShopItemView shopItemView in shopItemViews)
            {
                if (!_walletService.HasEnough(ItemTypeId, shopItemView.Price))
                    continue;

                _uiFactory.CreateElement<ShopItemView>(shopItemView, _parent);
            }
        }
    }
}