using System;
using System.Threading.Tasks;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.Providers.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.EnemySystem
{
    public class Enemy : MonoBehaviour
    {
        private Player _player;

        [Inject]
        private async void Construct(PlayerProvider playerProvider)
        {
            while (playerProvider.Player == null)
            {
                await UniTask.Yield();
            }

            _player = playerProvider.Player;
        }

        private void Update()
        {
            if (_player == null)
                return;

            Vector3 direction = (_player.transform.position - transform.position).normalized;

            var dot = Vector3.Dot(transform.forward, direction);

            var vector = Vector3.ProjectOnPlane(Camera.main.transform.forward, _player.transform.up);

            _player.transform.rotation = Quaternion.LookRotation(vector);
            
            Debug.DrawRay(_player.transform.up, Vector3.one, Color.blue);

            if (dot > 0.95f)
                print("LOSE");
        }
    }
}