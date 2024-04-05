using UnityEngine;

namespace CodeBase.Gameplay.PaperS
{
    public interface IHoldable
    {
        Transform Transform { get; }
        bool IsAccessed { get; }
        void Destroy();
    }
}