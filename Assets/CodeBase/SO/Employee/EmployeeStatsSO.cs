using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.SO.Employee
{
    [CreateAssetMenu(fileName = nameof(EmployeeStatsSO), menuName = "Gameplay/Employee stats")]
    public class EmployeeStatsSO : SerializedScriptableObject
    {
        public float PaperProcessTime = 5f;
        public float DecreasePaperProcessTime = 0.5f;
        public float MinPaperProcessTime = 1f;

        [Range(10, 100)] public int MinOfflineProfit = 50;
        [Range(50, 200)] public int MaxOfflineProfit = 150;
        [Range(3000, 6000)] public int UpgradeCost = 3000;
        [Range(1000, 3000)]  public int AdditionalUpgradeCost = 1000;
    }
}