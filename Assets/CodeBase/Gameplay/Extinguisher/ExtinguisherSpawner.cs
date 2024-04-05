using CodeBase.Animations;
using CodeBase.Constant;
using CodeBase.Services.Factories.GameItem;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Extinguisher
{
    public class ExtinguisherSpawner : MonoBehaviour
    {
        private GameItemFactory _gameItemFactory;
        private ExtinguisherSystem _createdExtinguisher;

        [Inject]
        private void Construct(GameItemFactory gameItemFactory) => 
            _gameItemFactory = gameItemFactory;

        public void DestroyCreatedExtinguisher() =>
            _createdExtinguisher.GetComponent<TransformScaleAnim>()
                .UnScale(() => Destroy(_createdExtinguisher.gameObject));

        [ContextMenu("Spawn")]
        [Button(ButtonSizes.Medium)]
        public void Spawn()
        {
            _createdExtinguisher = _gameItemFactory.Create<ExtinguisherSystem>(transform, transform.position,
                Quaternion.identity,
                AssetPath.Extinguisher);
            
            _createdExtinguisher.GetComponent<TransformScaleAnim>().ToScale();
        }
    }
}