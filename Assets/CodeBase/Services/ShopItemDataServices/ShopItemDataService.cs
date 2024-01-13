using CodeBase.Enums;
using CodeBase.Services.WorldData;

namespace CodeBase.Services.ShopItemDataServices
{
    public class ShopItemDataService
    {
        private readonly IWorldDataService _worldDataService;

        public ShopItemDataService(IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
        }

        public void Add(GameItemType gameItemType) => 
            _worldDataService.WorldData.ShopItemData.PurchasedShopItems.Add(gameItemType);

        public bool AlreadyPurchased(GameItemType gameItemType) =>
            _worldDataService.WorldData.ShopItemData.PurchasedShopItems.Contains(gameItemType);
    }
}