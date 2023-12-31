﻿using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.ShopItemData;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Shop
{
    public class ShopTabView : MonoBehaviour
    {
        [field: SerializeField] public ItemTypeId ItemTypeId { get; private set; }
        [SerializeField] private Transform _parent;

        private IAssetProvider _assetProvider;
        private ShopItemService _shopItemService;

        [Inject]
        private void Construct( IAssetProvider assetProvider, ShopItemService shopItemService)
        {
            _shopItemService = shopItemService;
            _assetProvider = assetProvider;
        }

        public void Init()
        {
            IEnumerable<ShopItemView> shopItemViews = _assetProvider.GetAll<ShopItemView>(AssetPath.ShopItemViews)
                .Where(x => x.ItemTypeId == ItemTypeId);

            foreach (ShopItemView shopItemView in shopItemViews)
            {
                if (_shopItemService.AlreadyPurchased(shopItemView.ShopItemTypeId))
                    continue;

                _shopItemService.CreateShopItemView(shopItemView, _parent);
            }
        }
    }
}