using System.Collections.Generic;
using CodeBase.Constant;
using CodeBase.Services.Factories.ShopItems;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Fire
{
    public class FireSpawner : MonoBehaviour
    {
        private GameItemFactory _gameItemFactory;

        [Inject]
        private void Construct(GameItemFactory gameItemFactory) => 
            _gameItemFactory = gameItemFactory;

        [Button]
        public void Init()
        {
            _gameItemFactory.Create<FireSystem>(transform, transform.position, transform.rotation, AssetPath.Fire);
        }
    }
}