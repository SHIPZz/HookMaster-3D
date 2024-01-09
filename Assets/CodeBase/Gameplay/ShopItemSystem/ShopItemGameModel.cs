using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Gameplay.ShopItemSystem
{
    public class ShopItemGameModel : MonoBehaviour
    {
        [field: SerializeField] public ShopItemTypeId ShopItemTypeId { get; private set; }
    }
}