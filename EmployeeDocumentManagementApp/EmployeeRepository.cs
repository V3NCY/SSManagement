// EmployeeRepository.cs
using System.Collections.Generic;
using System.Linq;

namespace EmployeeDocumentManagementApp
{
    public class EmployeeRepository
    {
        private static List<Employee> employees = new List<Employee>();

        public static List<Employee> Employees
        {
            get { return employees; }
        }

        public static void AddEmployee(Employee employee)
        {
            employees.Add(employee);
        }

        public static Employee GetEmployeeByName(string name)
        {
            return employees.FirstOrDefault(e => e.EmployeeName == name);
        }

        public static List<Employee> GetEmployees()
        {
            return employees;
        }

        public static void RemoveEmployee(Employee employee)
        {
            employees.Remove(employee);
        }
    }
}
