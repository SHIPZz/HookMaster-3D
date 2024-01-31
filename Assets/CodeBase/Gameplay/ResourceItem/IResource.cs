using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Gameplay.ResourceItem
{
    public interface IResource
    {
        ItemTypeId ItemTypeId { get; }
        int Value { get; }
        Transform Anchor { get; }
        void MarkAsDetected();
        void Collect();
    }
}