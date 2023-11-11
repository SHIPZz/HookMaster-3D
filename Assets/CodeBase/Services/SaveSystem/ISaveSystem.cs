using Cysharp.Threading.Tasks;

namespace CodeBase.Services.SaveSystem
{
    public interface ISaveSystem
    {
        void Save(CodeBase.Data.WorldData data);

        UniTask<CodeBase.Data.WorldData> Load();
    }
}