using System.Collections.Generic;
using System.Linq;
using CodeBase.SO.Office;
using UnityEngine;

namespace CodeBase.Services.DataService
{
    public class OfficeStaticDataService
    {
        private readonly Dictionary<int, OfficeSO> _officeDatas;

        public OfficeStaticDataService()
        {
            _officeDatas = Resources.LoadAll<OfficeSO>("Datas/Offices")
                .ToDictionary(x => x.QualificationType, x => x);
        }

        public OfficeSO Get(int qualificationType) =>
            _officeDatas[qualificationType];
    }
}