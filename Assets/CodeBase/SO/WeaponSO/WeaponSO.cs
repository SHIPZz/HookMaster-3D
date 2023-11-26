using CodeBase.Enums;
using CodeBase.Gameplay.BulletSystem;
using CodeBase.Gameplay.WeaponSystem;
using UnityEngine;

namespace CodeBase.SO.WeaponSO
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "Gameplay/WeaponSO")]
    public class WeaponSO : ScriptableObject
    {
        [Range(15, 350)] public float Speed = 25f;

        public int Damage;
        public WeaponTypeId WeaponTypeId;
        public Weapon WeaponPrefab;
        public Bullet BulletPrefab;
    }
}