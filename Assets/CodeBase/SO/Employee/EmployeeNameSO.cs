using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.SO.Employee
{
    [CreateAssetMenu(fileName = "EmployeeNameSO", menuName = "Gameplay/EmployeeNameSo")]
    public class EmployeeNameSO : ScriptableObject
    {
        public List<string> Names;
    }
}