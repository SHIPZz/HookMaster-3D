using CodeBase.Data;
using CodeBase.Gameplay.BurnableObjectSystem;
using UnityEngine;

namespace CodeBase.Extensions
{
    public static class DataExtension
    {
        public static VectorData ToData(this Vector3 vector) =>
            new VectorData(vector.x, vector.y, vector.z);


        public static Vector3 ToVector(this VectorData vectorData)
        {
            var vector = new Vector3(vectorData.X, vectorData.Y, vectorData.Z);
            return vector;
        }

        public static BurnableItemData ToData(this BurnableObject burnableObject)
        {
            BurnableItemData burnableItemData = new()
            {
                Position = burnableObject.transform.position.ToData(),
                IsBurned = burnableObject.IsBurned
            };
            
            return burnableItemData;
        }
    }
}