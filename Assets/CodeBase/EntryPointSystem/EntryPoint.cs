using CodeBase.Enums;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.Factories.Camera;
using CodeBase.Services.Factories.Player;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.Providers.Location;
using UnityEngine;
using Zenject;

namespace CodeBase.EntryPointSystem
{
    public class EntryPoint : IInitializable
    {
        private readonly LocationProvider _locationProvider;
        private readonly IPlayerFactory _playerFactory;
        private readonly ICameraFactory _cameraFactory;
        private readonly CameraProvider _cameraProvider;

        public EntryPoint(LocationProvider locationProvider, 
            IPlayerFactory playerFactory,
            ICameraFactory cameraFactory, 
            CameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
            _locationProvider = locationProvider;
            _playerFactory = playerFactory;
            _cameraFactory = cameraFactory;
        }

        public void Initialize()
        {
            Player player = _playerFactory.Create(PlayerTypeId.Wolverine, _locationProvider.Locations[LocationTypeId.PlayerSpawnPoint]);
            InitializeCamera(player);
            player.GetComponent<PlayerMovement>().SetCamera(_cameraProvider.Camera);
        }

        private void InitializeCamera(Player player)
        {
            CameraFollower cameraFollower = _cameraFactory.Create();
            cameraFollower.SetTarget(player.transform);
            _cameraProvider.CameraFollower = cameraFollower;
            _cameraProvider.Camera = cameraFollower.GetComponent<Camera>();
        }
    }
}