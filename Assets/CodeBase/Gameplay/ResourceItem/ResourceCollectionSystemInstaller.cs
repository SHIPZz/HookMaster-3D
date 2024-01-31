using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ResourceItem
{
    [CreateAssetMenu]
    internal class ResourceCollectionSystemInstaller : ScriptableObjectInstaller<ResourceCollectionSystemInstaller>
    {
        [SerializeField] private ResourceCollectionSettings _settings;
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ResourceCollectionSystem>().AsSingle().WithArguments(_settings);
        }
    }
}