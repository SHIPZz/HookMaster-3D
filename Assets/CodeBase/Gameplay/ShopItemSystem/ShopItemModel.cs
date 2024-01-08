using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Gameplay.ShopItemSystem
{
    public class ShopItemModel : MonoBehaviour
    {
        [field: SerializeField] public ShopItemTypeId ShopItemTypeId { get; private set; }
    }
}