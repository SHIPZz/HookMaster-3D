using UnityEngine;

namespace CodeBase.SO.GameItem.CircleRoulette
{
    [CreateAssetMenu(fileName = "CircleRouletteSO", menuName = "Gameplay/GameItem/CircleRouletteSO")]
    public class CircleRouletteSO : GameItemAbstractSO
    {
        [Range(100, 500)] public int MinWinValue;
        [Range(600, 1000)] public int MaxWinValue;
        
        
    }
}