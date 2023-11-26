using CodeBase.Enums;
using CodeBase.Gameplay.BulletSystem;
using CodeBase.Services.Data;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.Bullet
{
    public class BulletFactory : IBulletFactory
    {
        private readonly WeaponStaticDataService _weaponStaticDataService;
        private readonly DiContainer _diContainer;

        public BulletFactory(WeaponStaticDataService weaponStaticDataService, DiContainer diContainer)
        {
            _weaponStaticDataService = weaponStaticDataService;
            _diContainer = diContainer;
        }

        public Gameplay.BulletSystem.Bullet Create(WeaponTypeId weaponTypeId, Vector3 startPos)
        {
            Gameplay.BulletSystem.Bullet bulletPrefab = _weaponStaticDataService.Get(weaponTypeId).BulletPrefab;
            var targetBulletSpeed = _weaponStaticDataService.Get(weaponTypeId).Speed;
            var targetBulletDamage = _weaponStaticDataService.Get(weaponTypeId).Damage;
            
            var createdBullet = _diContainer.InstantiatePrefabForComponent<Gameplay.BulletSystem.Bullet>(bulletPrefab, 
                startPos,
                Quaternion.identity,
                null);

            createdBullet.GetComponent<BulletMovement>().SetSpeed(targetBulletSpeed);
            createdBullet.GetComponent<DamageDealer>().SetDamage(targetBulletDamage);
            
            return createdBullet;
        }
    }
}