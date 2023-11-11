using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Services.Factories.Player
{
    public interface IPlayerFactory
    {
        Gameplay.PlayerSystem.Player Create(PlayerTypeId playerTypeId, Transform spawnPoint);
    }
}