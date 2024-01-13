using CodeBase.Data;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Gameplay.GameItems;
using CodeBase.Gameplay.PurchaseableSystem;
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

        public static CircleRouletteItemData ToData(this CircleRouletteItem circleRouletteItem)
        {
            var circleRouletteItemData = new CircleRouletteItemData()
            {
                Id = circleRouletteItem.Id,
                PlayTime = circleRouletteItem.PlayTime,
                Position = circleRouletteItem.transform.position.ToData(),
                MaxWinValue = circleRouletteItem.MaxWinValue,
                MinWinValue = circleRouletteItem.MinWinValue,
            };

            return circleRouletteItemData;
        }

        public static PurchaseableItemData ToData(this PurchaseableItem purchaseableItem)
        {
            var purchaseableItemData = new PurchaseableItemData()
            {
                GameItemType = purchaseableItem.GameItemType,
                IsAccessible = purchaseableItem.IsAсcessible,
                Price = purchaseableItem.Price,
                Position = purchaseableItem.transform.position.ToData()
            };

            return purchaseableItemData;
        }

        public static MiningFarmData ToData(this MiningFarmItem miningFarmItem)
        {
            var miningFarmData = new MiningFarmData()
            {
                Id = miningFarmItem.Id,
                Position = miningFarmItem.transform.position.ToData(),
                WorkingMinutes = miningFarmItem.WorkingMinutes,
                ProfitPerMinute =  miningFarmItem.ProfitPerMinute
            };

            return miningFarmData;
        }
        
        public static Vector3 ToVector(this VectorData vectorData)
        {
            var vector = new Vector3(vectorData.X, vectorData.Y, vectorData.Z);
            return vector;
        }
    }
}