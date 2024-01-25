using CodeBase.Gameplay.Clients;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.DisableClientZoneSystem
{
    public class ClientOfficeZone : MonoBehaviour
    {
        [SerializeField] private float _targetDot = 0.5f;
        private TriggerObserver _triggerObserver;

        private void Awake() =>
            _triggerObserver = GetComponent<TriggerObserver>();

        private void OnEnable() =>
            _triggerObserver.TriggerEntered += OnClientEntered;

        private void OnDisable() =>
            _triggerObserver.TriggerEntered -= OnClientEntered;

        private void OnClientEntered(Collider client)
        {
            var dot = Vector3.Dot(transform.forward, client.transform.forward);
            print(dot);

            if (Mathf.Abs(dot) >= _targetDot)
                client.GetComponent<Client>().LeftOffice = true;
        }
    }
}