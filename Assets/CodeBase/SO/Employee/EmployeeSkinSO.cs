using System.Collections.Generic;
using System.Linq;
using CodeBase.Enums;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace CodeBase.SO.Employee
{
    [CreateAssetMenu(fileName = "EmployeeSkinSO", menuName = "Gameplay/EmployeeSkinSO")]
    public class EmployeeSkinSO : SerializedScriptableObject
    {
        [OdinSerialize] public Dictionary<EmployeeTypeId, Mesh> Skins;

        public Mesh Get(EmployeeTypeId employeeTypeId)
        {
            foreach (KeyValuePair<EmployeeTypeId, Mesh> keyValuePair in Skins)
            {
                if (keyValuePair.Key == employeeTypeId)
                    return keyValuePair.Value;
            }

            return null;
        }
    }
}