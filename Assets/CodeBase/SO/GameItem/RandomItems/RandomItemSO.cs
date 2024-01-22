using CodeBase.Enums;
using CodeBase.Gameplay.GameItems;
using UnityEngine;

namespace CodeBase.SO.GameItem.RandomItems
{
    [CreateAssetMenu(fileName = "RandomItemSO", menuName = "Gameplay/GameItemSO/RandomItemSO")]
    public class RandomItemSO : ScriptableObject
    {
        public GameItemType GameItemType;
        public RandomItem Prefab;
        public int Profit;
        public string Name;
        public Sprite Icon;
        public Vector2 IconPosition;
        public Vector2 Width;
    }
}