using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.UI.Roulette;

namespace CodeBase.Services.Reward
{
    public class RewardService
    {
        private Dictionary<RouletteItemTypeId, int> _rouletteRewards = new()
        {
            { RouletteItemTypeId.Money , 0},
            { RouletteItemTypeId.Ticket , 0},
            { RouletteItemTypeId.Diamond , 0},
        };

        public IReadOnlyDictionary<RouletteItemTypeId, int> RouletteRewards => _rouletteRewards;

        public void AddRouletteReward(RouletteItem rouletteItem) => 
            _rouletteRewards[rouletteItem.RouletteItemTypeId] = rouletteItem.Quantity;
    }
}