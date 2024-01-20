using System;
using CodeBase.Services.Clients;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Clients
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private ClientMovement _clientMovement;
        
        public string Id;
        public bool IsServed;

        public void SetTarget(Transform target)
        {
            _clientMovement.SetTarget(target);
        }

        public void MoveBack()
        {
            _clientMovement.MoveBack();
            print("moveback" + name);
        }

        public void SetSitIdle(bool isSitIdle, Transform target)
        {
            _clientMovement.SetTarget(target);
            _clientMovement.SetSitIdle(isSitIdle);
        }

        public void SetSitIdleByMoving(Transform target)
        {
            _clientMovement.SetTarget(target);
            _clientMovement.SetSitIdleByMoving(target);
        }

        [Button]
        private void CreateId() => Id = Guid.NewGuid().ToString();
    }
}