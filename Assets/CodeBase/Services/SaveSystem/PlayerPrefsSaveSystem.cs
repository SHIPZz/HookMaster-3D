using Cysharp.Threading.Tasks;
using fastJSON;
using UltimateJson;
using UnityEngine;

namespace CodeBase.Services.SaveSystem
{
    public class PlayerPrefsSaveSystem : ISaveSystem
    {
        public void Save(CodeBase.Data.WorldData data)
        {
            string jsonData = JsonObject.Serialise(data);
            PlayerPrefs.SetString(typeof(CodeBase.Data.WorldData).FullName, jsonData);
            PlayerPrefs.Save();
        }
        
        public async UniTask<CodeBase.Data.WorldData> Load()
        {
            if (PlayerPrefs.HasKey(typeof(CodeBase.Data.WorldData).FullName))
            {
                string jsonData = PlayerPrefs.GetString(typeof(CodeBase.Data.WorldData).FullName);
                return JsonObject.Deserialise<CodeBase.Data.WorldData>(jsonData);
            }
            
            await UniTask.Yield();
            
            return new CodeBase.Data.WorldData();
        }
    }
}