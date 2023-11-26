using System;
using CodeBase.Services.Providers.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.WeaponSystem
{
    public class WeaponMediator : IInitializable, IDisposable
    {
        private readonly PlayerProvider _playerProvider;
        private readonly Weapon _weapon;

        public WeaponMediator(PlayerProvider playerProvider, Weapon weapon)
        {
            _weapon = weapon;
            _playerProvider = playerProvider;
        }

        public void Initialize()
        {

        }

        public void Dispose()
        {
        }
        
    }
}