using CodeBase.Data;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Gameplay.TableSystem;
using UnityEngine;

namespace CodeBase.Extensions
{
    public static class DataExtension
    {
        public static VectorData ToData(this Vector3 vector) =>
            new VectorData(vector.x, vector.y, vector.z);


        public static TableData ToData(this Table table)
        {
            var tableData = new TableData()
            {
                Id = table.Id,
                IsBurned = table.IsBurned,
                IsFree = table.IsFree
            };

            return tableData;
        }
        
        public static Vector3 ToVector(this VectorData vectorData)
        {
            var vector = new Vector3(vectorData.X, vectorData.Y, vectorData.Z);
            return vector;
        }
    }
}