using UnityEngine;

namespace _Project_legacy.Scripts.Papers
{
    public interface IHoldable
    {
        Transform Transform { get; }
        bool IsAccessed { get; }
        void Destroy();
    }
}