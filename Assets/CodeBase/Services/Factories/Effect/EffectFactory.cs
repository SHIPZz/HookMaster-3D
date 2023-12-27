using CodeBase.Enums;
using CodeBase.Gameplay.Effects;
using CodeBase.Services.DataService;
using CodeBase.Services.Providers.Location;
using Zenject;

namespace CodeBase.Services.Factories.Effect
{
    public interface IEffectFactory
    {
        EffectView Create(EffectTypeId effectTypeId);
    }

    public class EffectFactory : IEffectFactory
    {
        private readonly EffectStaticDataService _effectStaticDataService;
        private readonly DiContainer _diContainer;
        private readonly LocationProvider _locationProvider;

        public EffectFactory(EffectStaticDataService effectStaticDataService, DiContainer diContainer,
            LocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
            _effectStaticDataService = effectStaticDataService;
            _diContainer = diContainer;
        }
        
        public EffectView Create(EffectTypeId effectTypeId)
        {
            EffectView effectPrefab = _effectStaticDataService.Get(effectTypeId);
            return _diContainer.InstantiatePrefabForComponent<EffectView>(effectPrefab, _locationProvider.EffectParent);
        }
        
    }
}