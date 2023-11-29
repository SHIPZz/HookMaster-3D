using UnityEngine;

[CreateAssetMenu(fileName = "OfficeSO", menuName = "Gameplay/OfficeSO")]
public class OfficeSO : ScriptableObject
{
    [Range(10000, 1000000)] public int MinSalary = 10000;
    [Range(20000, 1000000)] public int MaxSalary = 20000;
    [Range(10000, 1000000)] public int MinProfit = 10000;
    [Range(20000, 1000000)] public int MaxProfit = 20000;
    public int QualificationType = 1;
}