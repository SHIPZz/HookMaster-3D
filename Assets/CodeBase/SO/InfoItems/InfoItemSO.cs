using UnityEngine;

namespace CodeBase.SO.InfoItems
{
    [CreateAssetMenu(fileName = "InfoItem", menuName = "Gameplay/Data/InfoItem")]
    public class InfoItemSO : ScriptableObject
    {
        public InfoItemTypeId InfoItemTypeId;
        public string Description;
        public string Name;
        public Sprite Icon;
    }

    public enum InfoItemTypeId
    {
        ClientServeRoom,
    }
}