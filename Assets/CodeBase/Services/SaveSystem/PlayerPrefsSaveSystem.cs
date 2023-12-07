using Cysharp.Threading.Tasks;
using UltimateJson;
using UnityEngine;

namespace CodeBase.Services.SaveSystem
{
    public class PlayerPrefsSaveSystem : ISaveSystem
    {
        private const string DataKey = "Data";
        
        public void Save(Data.WorldData data)
        {
            string jsonData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(DataKey, jsonData);
            PlayerPrefs.Save();
        }
        
        public async UniTask<CodeBase.Data.WorldData> Load()
        {
            if (PlayerPrefs.HasKey(DataKey))
            {
                string jsonData = PlayerPrefs.GetString(DataKey);
                return JsonUtility.FromJson<CodeBase.Data.WorldData>(jsonData);
            }
            
            await UniTask.Yield();
            
            return new CodeBase.Data.WorldData();
        }
    }
}