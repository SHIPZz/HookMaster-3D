using System;
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

        [Inject]
        private void Construct(ClientMovement clientMovement)
        {
            _clientMovement = clientMovement;
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

        [Button]
        private void CreateId() => Id = Guid.NewGuid().ToString();
    }
}