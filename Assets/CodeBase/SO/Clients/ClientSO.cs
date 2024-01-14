using CodeBase.SO.GameItem;
using UnityEngine;

namespace CodeBase.SO.Clients
{
    [CreateAssetMenu(fileName = nameof(ClientSO), menuName = "Gameplay/Client/SO")]
    public class ClientSO : GameItemAbstractSO
    {
        [Range(0, 10000)] public int ServeProfit;
        [Range(0, 1000)] public float ServeTime;
    }
}