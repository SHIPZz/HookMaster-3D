using CodeBase.Enums;
using CodeBase.Gameplay.WeaponSystem;
using CodeBase.Services.Data;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.Weapon
{
    public class WeaponFactory : IWeaponFactory
    {
        private readonly WeaponStaticDataService _weaponStaticDataService;
        private readonly DiContainer _diContainer;

        public WeaponFactory(WeaponStaticDataService weaponStaticDataService, DiContainer diContainer)
        {
            _weaponStaticDataService = weaponStaticDataService;
            _diContainer = diContainer;
        }

        public Gameplay.WeaponSystem.Weapon Create(WeaponTypeId weaponTypeId, Transform parent, Vector3 startPos)
        {
            var weaponPrefab = _weaponStaticDataService.Get(weaponTypeId).WeaponPrefab;

            return   _diContainer.InstantiatePrefabForComponent<Gameplay.WeaponSystem.Weapon>(weaponPrefab,
                startPos,
                Quaternion.identity,
                parent);
        }
    }
}