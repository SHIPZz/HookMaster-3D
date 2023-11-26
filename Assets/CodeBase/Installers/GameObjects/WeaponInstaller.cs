using CodeBase.Gameplay.WeaponSystem;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers.GameObjects
{
    [RequireComponent(typeof(GameObjectContext))]
    public class WeaponInstaller : MonoInstaller
    {
        [SerializeField] private LayerMask _raycastLayerMask;
        
        public override void InstallBindings()
        {
            Container.BindInstance(GetComponent<Weapon>());
            Container.BindInterfacesAndSelfTo<WeaponMediator>().AsSingle();
            Container.BindInstance(_raycastLayerMask);
        }
    }
}