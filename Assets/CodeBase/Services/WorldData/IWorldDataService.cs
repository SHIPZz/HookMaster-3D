using Cysharp.Threading.Tasks;

namespace CodeBase.Services.WorldData
{
    public interface IWorldDataService
    {
        UniTask Load();
        void Save();
        CodeBase.Data.WorldData WorldData { get; }
        void Reset();
    }
}