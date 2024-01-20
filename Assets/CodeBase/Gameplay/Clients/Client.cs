using System;
using CodeBase.Services.Clients;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Clients
{
    public class Client : MonoBehaviour
    {
        public string Id;
        public bool IsServed;

        private ClientMovement _clientMovement;
        private ClientObjectService _clientObjectService;

        [Inject]
        private void Construct(ClientMovement clientMovement, ClientObjectService clientObjectService)
        {
            _clientObjectService = clientObjectService;
            _clientMovement = clientMovement;
        }

        private void OnDisable()
        {
            _clientObjectService.CountDisabledClients();
        }

        public void SetTarget(Transform target)
        {
            _clientMovement.SetTarget(target);
        }

        public void MoveBack(Action onComplete = null)
        {
            _clientMovement.MoveBack(onComplete);
        }

        public bool IsMovingBack() => 
            _clientMovement.IsMovingBack();

        public void SetSitIdle(bool isSitIdle, Transform target)
        {
            _clientMovement.SetTarget(target);
            _clientMovement.SetSitIdle(isSitIdle);
        }

        public void SetSitIdleByMoving(Transform target)
        {
            _clientMovement.SetTarget(target);
            _clientMovement.SetSitIdleByMoving();
        }

        [Button]
        private void CreateId() => Id = Guid.NewGuid().ToString();
    }
}