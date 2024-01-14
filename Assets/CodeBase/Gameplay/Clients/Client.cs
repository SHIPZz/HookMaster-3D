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

        public void StartMovement(Vector3 target)
        {
            _clientMovement.SetTarget(target);
        }

        public void SetSitIdle(bool isSitIdle, Transform target)
        {
            _clientMovement.SetSitIdle(isSitIdle, target,this);
        }

        [Button]
        private void CreateId() => Id = Guid.NewGuid().ToString();
    }
}