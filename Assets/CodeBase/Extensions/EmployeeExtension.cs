using CodeBase.Data;
using CodeBase.Gameplay.EmployeeSystem;

namespace CodeBase.Extensions
{
    public static class EmployeeExtension
    {
        public static EmployeeData ToEmployeeData(this Employee employee)
        {
            var potentialEmployeeData = new EmployeeData
            {
                Guid = employee.Guid,
                Profit = employee.Profit,
                Name = employee.Name,
                Salary = employee.Salary,
                QualificationType = employee.QualificationType,
                TableId = employee.TableId
            };
            
            return potentialEmployeeData;
        }
    }
}