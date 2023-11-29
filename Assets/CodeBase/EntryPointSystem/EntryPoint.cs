using CodeBase.Enums;
using CodeBase.Gameplay.Camera;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.Factories.Camera;
using CodeBase.Services.Factories.Player;
using CodeBase.Services.Factories.Weapon;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Providers.Player;
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
        private readonly PlayerProvider _playerProvider;
        private IWeaponFactory _weaponFactory;

        public EntryPoint(LocationProvider locationProvider, 
            IPlayerFactory playerFactory,
            ICameraFactory cameraFactory, 
            CameraProvider cameraProvider,
            PlayerProvider playerProvider,
            IWeaponFactory weaponFactory)
        {
            _weaponFactory = weaponFactory;
            _playerProvider = playerProvider;
            _cameraProvider = cameraProvider;
            _locationProvider = locationProvider;
            _playerFactory = playerFactory;
            _cameraFactory = cameraFactory;
        }

        public void Initialize()
        {
            Player player = _playerFactory.Create(PlayerTypeId.Wolverine, _locationProvider.Locations[LocationTypeId.PlayerSpawnPoint]);
            InitializeCamera(player);
            _playerProvider.Player = player;
            _weaponFactory.Create(WeaponTypeId.DefaultStretchingRope, player.transform, player.transform.position);
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