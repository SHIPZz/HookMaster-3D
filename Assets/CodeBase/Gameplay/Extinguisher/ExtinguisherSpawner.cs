using System;
using CodeBase.Constant;
using CodeBase.Services.Factories.ShopItems;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Extinguisher
{
    public class ExtinguisherSpawner : MonoBehaviour
    {
        private GameItemFactory _gameItemFactory;

        [Inject]
        private void Construct(GameItemFactory gameItemFactory) => 
            _gameItemFactory = gameItemFactory;

        private void Start()
        {
            Spawn();
        }

        public void Spawn()
        {
            _gameItemFactory.Create<ExtinguisherSystem>(transform, transform.position, Quaternion.identity,
                AssetPath.Extinguisher);
        }
    }
}