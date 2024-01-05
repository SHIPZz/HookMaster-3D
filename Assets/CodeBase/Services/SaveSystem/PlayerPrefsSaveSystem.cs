using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UltimateJson;
using UnityEngine;

namespace CodeBase.Services.SaveSystem
{
    public class PlayerPrefsSaveSystem : ISaveSystem
    {
        private const string DataKey = "Data";
        
        public void Save(Data.WorldData data)
        {
            string jsonData = JsonConvert.SerializeObject(data,Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            
            PlayerPrefs.SetString(DataKey, jsonData);
            PlayerPrefs.Save();
        }
        
        public async UniTask<CodeBase.Data.WorldData> Load()
        {
            if (PlayerPrefs.HasKey(DataKey))
            {
                string jsonData = PlayerPrefs.GetString(DataKey);
                return JsonConvert.DeserializeObject<Data.WorldData>(jsonData);
            }
            
            await UniTask.Yield();
            
            return new CodeBase.Data.WorldData();
        }
    }
}