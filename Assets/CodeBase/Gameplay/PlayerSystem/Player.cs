using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public CharacterTypeId CharacterTypeId { get; private set; }
    }
}