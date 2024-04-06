using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Wallet;
using CodeBase.UI.Roulette;

namespace CodeBase.Services.Reward
{
    public class RewardService
    {
        private Dictionary<ItemTypeId, int> _rouletteRewards = new()
        {
            { ItemTypeId.Money , 0},
            { ItemTypeId.Ticket , 0},
            { ItemTypeId.Diamond , 0},
        };

        private readonly WalletService _walletService;

        public IReadOnlyDictionary<ItemTypeId, int> RouletteRewards => _rouletteRewards;

        public RewardService(WalletService walletService)
        {
            _walletService = walletService;
        }

        public void Add(ItemTypeId itemTypeId, int amount)
        {
            _rouletteRewards[itemTypeId] += amount;
            _walletService.Set(itemTypeId, amount);
            
        }

        public void AddRouletteReward(RouletteItem rouletteItem)
        {
            Add(rouletteItem.ItemTypeId, rouletteItem.Quantity);
        }
    }
}