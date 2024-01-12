using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.MaterialChanger;
using CodeBase.Services.WorldData;

namespace CodeBase.Services.BurnableObjects
{
    public class BurnableObjectService
    {
        private List<IBurnable> _burnables = new();
        private readonly IWorldDataService _worldDataService;
        private RendererMaterialChangerService _rendererMaterialChangerService;

        public BurnableObjectService(IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
        }

        public void Add(IBurnable burnable)
        {
            _burnables.Add(burnable);
        }

        public void Change()
        {
            _rendererMaterialChangerService.Change();
        }

        // public void TryAdd(BurnableItemData burnableItem)
        // {
        //     if (!_worldDataService.WorldData.BurnableItemDatas.Any(x =>
        //             ArePositionsEqual(x.Position, burnableItem.Position)))
        //     {
        //         _worldDataService.WorldData.BurnableItemDatas.Add(burnableItem);
        //     }
        // }
        //
        // public void SetIsBurned(BurnableItemData burnableItemData)
        // {
        //     var itemToUpdate = _worldDataService.WorldData.BurnableItemDatas
        //         .FirstOrDefault(x => ArePositionsEqual(x.Position, burnableItemData.Position));
        //
        //     if (itemToUpdate == null)
        //     {
        //         _worldDataService.WorldData.BurnableItemDatas.Add(burnableItemData);
        //         itemToUpdate = burnableItemData;
        //     }
        //     
        //     itemToUpdate.IsBurned = burnableItemData.IsBurned;
        //     _worldDataService.Save();
        // }
        //
        // public BurnableItemData GetTargetData(BurnableItemData burnableItemData)
        // {
        //     return _worldDataService.WorldData.BurnableItemDatas
        //         .FirstOrDefault(x => ArePositionsEqual(x.Position, burnableItemData.Position));
        // }
        //
        // private bool ArePositionsEqual(VectorData position1, VectorData position2)
        // {
        //     return Math.Abs(position1.X - position2.X) < 0.1f &&
        //            Math.Abs(position1.Y - position2.Y) < 0.1f &&
        //            Math.Abs(position1.Z - position2.Z) < 0.1f;
        // }
    }
}