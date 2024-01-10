using CodeBase.Gameplay.PlayerSystem;
using UnityEngine;

namespace CodeBase.Services.Providers.Player
{
    public class PlayerProvider
    {
        public PlayerInput PlayerInput;
        public Transform RightHand;
        public Gameplay.PlayerSystem.Player Player;
        public PlayerMovement PlayerMovement;
    }
}