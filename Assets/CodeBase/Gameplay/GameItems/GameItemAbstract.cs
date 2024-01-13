using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Gameplay.GameItems
{
    public class GameItemAbstract : MonoBehaviour
    {
        [field: SerializeField] public GameItemType GameItemType { get; private set; }
    }
}