using CodeBase.Constant;
using CodeBase.Services.Providers.Asset;
using Zenject;

namespace CodeBase.Services.Factories.Camera
{
    public class CameraFactory : ICameraFactory
    {
        private readonly DiContainer _diContainer;
        private readonly IAssetProvider _assetProvider;

        public CameraFactory(DiContainer diContainer, IAssetProvider assetProvider)
        {
            _diContainer = diContainer;
            _assetProvider = assetProvider;
        }

        public CameraFollower Create()
        {
            var cameraPrefab = _assetProvider.Get<CameraFollower>(AssetPath.Camera);
            return _diContainer.InstantiatePrefabForComponent<CameraFollower>(cameraPrefab.gameObject);
        }
    }
}