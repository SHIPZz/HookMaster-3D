using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Gameplay.ResourceItem;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.PaperSystem
{
    public class Paper : Resource
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
            Destroy(gameObject);

        private void OnPlayerEntered(Collider collider)
        {
            if (!collider.gameObject.TryGetComponent(out PlayerPaperContainer playerPaperContainer))
                return;

            transform.SetParent(collider.transform);
        }
    }
}