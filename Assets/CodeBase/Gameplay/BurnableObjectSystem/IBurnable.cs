using CodeBase.Data;

namespace CodeBase.Gameplay.BurnableObjectSystem
{
    public interface IBurnable
    {
        MaterialTypeId BurnMaterial { get; }
        bool IsBurned { get; }
        void Burn();
        void Recover();
    }
}