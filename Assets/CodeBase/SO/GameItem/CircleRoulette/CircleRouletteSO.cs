using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.SO.GameItem.CircleRoulette
{
    [CreateAssetMenu(fileName = "CircleRouletteSO", menuName = "Gameplay/GameItemSO/CircleRouletteSO")]
    public class CircleRouletteSO : PopupAbstractSO
    {
        [Range(100, 500)] public int MinWinValue;
        [Range(600, 1000)] public int MaxWinValue;
        [Range(5, 10)] public int PlayTime = 5;
        [Range(3000, 10000)] public int Price;
    }
}