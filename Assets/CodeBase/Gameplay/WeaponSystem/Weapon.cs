using CodeBase.Enums;
using CodeBase.Services.DataService;
using CodeBase.Services.Factories.Bullet;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.WeaponSystem
{
    public class Weapon : MonoBehaviour
    {
        private WeaponStaticDataService _weaponStaticDataService;
        private IBulletFactory _bulletFactory;
        [field: SerializeField] public WeaponTypeId WeaponTypeId { get; private set; }

        [Inject]
        private void Construct(IBulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory;
        }

        public void Shoot(Vector3 direction, Vector3 startPosition)
        {
            var targetDirection = (direction - transform.position).normalized;
            
            var bullet = _bulletFactory.Create(WeaponTypeId, transform.position);
            bullet.StartMoving(targetDirection,startPosition);
        }
    }
}