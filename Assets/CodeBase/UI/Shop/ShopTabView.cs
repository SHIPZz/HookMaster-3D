using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Asset;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Shop
{
    public class ShopTabView : MonoBehaviour
    {
        [field: SerializeField] public WalletValueTypeId WalletValueTypeId { get; private set; }
        [SerializeField] private Transform _parent;

        private UIFactory _uiFactory;
        private IAssetProvider _assetProvider;

        [Inject]
        private void Construct(UIFactory uiFactory, IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _uiFactory = uiFactory;
        }

        public void Init()
        {
            IEnumerable<ShopItemView> shopItemViews = _assetProvider
                .GetAll<ShopItemView>(AssetPath.ShopItemViews)
                .Where(x => x.WalletValueTypeId == WalletValueTypeId);

            foreach (ShopItemView shopItemView in shopItemViews)
            {
                _uiFactory.CreateElement<ShopItemView>(shopItemView, _parent);
            }
        }
    }
}