using System;
using CodeBase.Data;

namespace CodeBase.Gameplay.BurnableObjectSystem
{
    public interface IBurnable
    {
        MaterialTypeId BurnMaterial { get; }
        bool IsBurned { get; }
        event Action<IBurnable> Burned;
        void Burn();
        void Recover();
    }
}