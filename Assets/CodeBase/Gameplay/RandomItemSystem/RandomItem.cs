using CodeBase.Data;
using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Gameplay.RandomItemSystem
{
    public class RandomItem : MonoBehaviour
    {
        [field: SerializeField] public int Value { get; private set; }
        [field: SerializeField] public ItemTypeId ItemTypeId { get; private set; }
        [field: SerializeField] public RandomItemTypeId RandomItemTypeId { get; private set; }
    }
}
