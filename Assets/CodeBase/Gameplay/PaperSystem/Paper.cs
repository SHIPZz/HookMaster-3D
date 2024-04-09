using CodeBase.Gameplay.GameItems;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.PaperSystem
{
    public class Paper : GameItemAbstract
    {
      [field: SerializeField]  public bool ReadyToTransfer { get; set; }
        
        private TriggerObserver _triggerObserver;

        private void Awake() =>
            _triggerObserver = GetComponent<TriggerObserver>();

        private void OnEnable() =>
            _triggerObserver.TriggerEntered += OnPlayerEntered;

        private void OnDisable() =>
            _triggerObserver.TriggerEntered -= OnPlayerEntered;

        public void Destroy() => 
            DestroyImmediate(gameObject);

        private void OnPlayerEntered(Collider collider)
        {
            if (!collider.gameObject.TryGetComponent(out PlayerPaperContainer playerPaperContainer))
                return;

            transform.SetParent(collider.transform);
        }
    }
}