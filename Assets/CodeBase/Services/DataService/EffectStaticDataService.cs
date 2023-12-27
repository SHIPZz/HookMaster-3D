using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Gameplay.Effects;
using UnityEngine;

namespace CodeBase.Services.DataService
{
    public class EffectStaticDataService
    {
        private readonly Dictionary<EffectTypeId, EffectView> _effectViews;

        public EffectStaticDataService()
        {
            _effectViews = Resources.LoadAll<EffectView>(AssetPath.EffectView)
                .ToDictionary(x => x.EffectTypeId, x => x);
        }

        public EffectView Get(EffectTypeId effectTypeId) => 
            _effectViews[effectTypeId];
        

        public List<EffectTypeId> Get() =>
            _effectViews.Keys.ToList();
    }
}