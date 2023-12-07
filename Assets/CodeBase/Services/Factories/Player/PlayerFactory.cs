using CodeBase.Enums;
using CodeBase.Services.DataService;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.Player
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly DiContainer _diContainer;
        private readonly PlayerStaticDataService _playerStaticDataService;

        public PlayerFactory(DiContainer diContainer,  PlayerStaticDataService playerStaticDataService)
        {
            _playerStaticDataService = playerStaticDataService;
            _diContainer = diContainer;
        }
        
        public Gameplay.PlayerSystem.Player Create(PlayerTypeId playerTypeId, Transform spawnPoint)
        {
            Gameplay.PlayerSystem.Player playerPrefab = _playerStaticDataService.Get(playerTypeId).Prefab;
            
            return _diContainer.InstantiatePrefabForComponent<Gameplay.PlayerSystem.Player>(playerPrefab,
                spawnPoint.position, Quaternion.identity, null);
        }
    }
}