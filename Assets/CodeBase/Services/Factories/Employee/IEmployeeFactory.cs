using CodeBase.Data;
using CodeBase.Gameplay.TableSystem;

namespace CodeBase.Services.Factories.Employee
{
    public interface IEmployeeFactory
    {
        Gameplay.Employees.Employee Create(EmployeeData employeeData, Table targetTable, bool isSit);

        EmployeeData Create();
    }
}