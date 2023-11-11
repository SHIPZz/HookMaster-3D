using UnityEngine;

namespace CodeBase.Services.Providers.Asset
{
    public class AssetProvider : IAssetProvider
    {
        public T Get<T>(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return prefab.GetComponent<T>();
        }
    }
}