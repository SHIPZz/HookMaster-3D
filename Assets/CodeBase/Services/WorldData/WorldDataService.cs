using CodeBase.Services.SaveSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.WorldData
{
    public class WorldDataService : IWorldDataService
    {
        private readonly ISaveSystem _saveSystem;
        
        public CodeBase.Data.WorldData WorldData { get; private set; }

        public WorldDataService(ISaveSystem saveSystem) => 
            _saveSystem = saveSystem;

        public async UniTask Load()
        {
            WorldData = await _saveSystem.Load();
        }

        public void Reset()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            WorldData = null;
            WorldData = new();
            Save();
        }

        public void Save() => 
            _saveSystem.Save(WorldData);
    }
}