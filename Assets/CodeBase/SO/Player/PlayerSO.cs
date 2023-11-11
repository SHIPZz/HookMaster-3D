using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.SO.Player
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "Gameplay/PlayerSO")]
    public class PlayerSO : ScriptableObject
    {
        public PlayerTypeId PlayerTypeId;
        public Gameplay.PlayerSystem.Player Prefab;
    }
}