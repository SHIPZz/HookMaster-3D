using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.UI.Shop
{
    public class ShopItemView : MonoBehaviour
    {
        [field: SerializeField] public WalletValueTypeId WalletValueTypeId { get; private set; }
    }
}