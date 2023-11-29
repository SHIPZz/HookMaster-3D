using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Services.Data
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