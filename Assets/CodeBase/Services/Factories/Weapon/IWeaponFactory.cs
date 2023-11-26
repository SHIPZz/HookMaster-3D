using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Services.Factories.Weapon
{
    public interface IWeaponFactory
    {
        Gameplay.WeaponSystem.Weapon Create(WeaponTypeId weaponTypeId, Transform parent, Vector3 startPos);
    }
}