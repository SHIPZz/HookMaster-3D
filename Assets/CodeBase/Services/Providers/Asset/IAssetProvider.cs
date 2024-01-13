using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Services.Providers.Asset
{
    public interface IAssetProvider
    {
        T Get<T>(string path);
        List<T> GetAll<T>(string path) where T : Object;

        public T GetObject<T>(string path) where T : Object;
    }
}