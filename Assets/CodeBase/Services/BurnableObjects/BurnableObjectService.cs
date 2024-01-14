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
        public BurnableObjectService(IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
        }

        public void Add(IBurnable burnable)
        {
            _burnables.Add(burnable);
        }
    }
}