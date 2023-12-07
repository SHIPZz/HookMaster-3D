using System.Collections.Generic;
using System.Linq;
using CodeBase.Enums;
using CodeBase.SO.WeaponSO;
using UnityEngine;

namespace CodeBase.Services.DataService
{
    public class WeaponStaticDataService
    {
        private readonly Dictionary<WeaponTypeId, WeaponSO> _weaponDatas;

        public WeaponStaticDataService()
        {
            _weaponDatas = Resources.LoadAll<WeaponSO>("Datas/Weapon")
                .ToDictionary(x => x.WeaponTypeId, x => x);
        }

        public WeaponSO Get(WeaponTypeId weaponTypeId) =>
            _weaponDatas[weaponTypeId];
    }
}