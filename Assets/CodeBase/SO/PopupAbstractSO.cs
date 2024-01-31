using CodeBase.Enums;
using CodeBase.SO.GameItem;
using UnityEngine;

namespace CodeBase.SO
{
    public abstract class PopupAbstractSO : GameItemAbstractSO
    {
        [field: SerializeField] public GameItemType GameItemType { get; private set; }
        public string Name;
        public string Description;
        public Sprite Sprite;
    }
}