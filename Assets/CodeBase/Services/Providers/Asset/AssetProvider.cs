using System.Collections.Generic;
using System.Linq;
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

        public List<T> GetAll<T>(string path) where T : Object
        {
            List<T> prefabs = Resources.LoadAll<T>(path).ToList();
            return prefabs;
        }
    }
}