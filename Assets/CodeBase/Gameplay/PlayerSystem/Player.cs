using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public PlayerTypeId PlayerTypeId { get; private set; }
    }
}