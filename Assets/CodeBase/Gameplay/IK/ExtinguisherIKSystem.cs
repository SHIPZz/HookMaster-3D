using CodeBase.Services.Fire;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.IK
{
    public class ExtinguisherIKSystem : IKObjectSystem
    {
        private FireService _fireService;

        [Inject]
        private void Construct(FireService fireService)
        {
            _fireService = fireService;
        }
        
        protected override void OnPlayerEntered(Collision player)
        {
            if(!_fireService.IsFired)
                return;
            
            base.OnPlayerEntered(player);
        }
    }
}