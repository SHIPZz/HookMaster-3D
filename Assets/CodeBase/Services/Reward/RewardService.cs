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

        private WalletService _walletService;

        public RewardService(WalletService walletService)
        {
            _walletService = walletService;
        }

        public IReadOnlyDictionary<ItemTypeId, int> RouletteRewards => _rouletteRewards;

        public void AddRouletteReward(RouletteItem rouletteItem)
        {
            _rouletteRewards[rouletteItem.ItemTypeId] += rouletteItem.Quantity;
            _walletService.Set(rouletteItem.ItemTypeId, rouletteItem.Quantity);
        }
    }
}