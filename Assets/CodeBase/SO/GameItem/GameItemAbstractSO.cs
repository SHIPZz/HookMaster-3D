using UnityEngine;

namespace CodeBase.SO.GameItem
{
    public abstract class GameItemAbstractSO : ScriptableObject
    {
      [field: SerializeField] public  GameObject Prefab { get; protected set; }
    }
}