using System;
using System.Linq;
using CodeBase.Data;
using CodeBase.Services.WorldData;

namespace CodeBase.Services.BurnableObjects
{
    public class BurnableObjectService
    {
        private readonly IWorldDataService _worldDataService;

        public BurnableObjectService(IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
        }

        public void TryAdd(BurnableItemData burnableItem)
        {
            if (!_worldDataService.WorldData.BurnableItemDatas.Any(x =>
                    ArePositionsEqual(x.Position, burnableItem.Position)))
            {
                _worldDataService.WorldData.BurnableItemDatas.Add(burnableItem);
            }
        }

        public void SetIsBurned(BurnableItemData burnableItemData)
        {
            var itemToUpdate = _worldDataService.WorldData.BurnableItemDatas
                .FirstOrDefault(x => ArePositionsEqual(x.Position, burnableItemData.Position));

            if (itemToUpdate == null)
            {
                _worldDataService.WorldData.BurnableItemDatas.Add(burnableItemData);
                itemToUpdate = burnableItemData;
            }
            
            itemToUpdate.IsBurned = burnableItemData.IsBurned;
            _worldDataService.Save();
        }

        public BurnableItemData GetTargetData(BurnableItemData burnableItemData)
        {
            return _worldDataService.WorldData.BurnableItemDatas
                .FirstOrDefault(x => ArePositionsEqual(x.Position, burnableItemData.Position));
        }

        private bool ArePositionsEqual(VectorData position1, VectorData position2)
        {
            return Math.Abs(position1.X - position2.X) < 0.1f &&
                   Math.Abs(position1.Y - position2.Y) < 0.1f &&
                   Math.Abs(position1.Z - position2.Z) < 0.1f;
        }
    }
}