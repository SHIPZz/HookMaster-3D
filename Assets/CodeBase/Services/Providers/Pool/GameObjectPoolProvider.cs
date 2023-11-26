using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Services.GOPool;

namespace CodeBase.Services.Providers.Pool
{
    public class GameObjectPoolProvider
    {
        public Dictionary<WeaponTypeId, GameObjectPool> _bulletPools = new();
    }
}