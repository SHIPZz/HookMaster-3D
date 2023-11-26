using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Services.Factories.Bullet
{
    public interface IBulletFactory
    {
        public Gameplay.BulletSystem.Bullet Create(WeaponTypeId weaponTypeId, Vector3 startPos);
    }
}