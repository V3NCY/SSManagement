using System.Collections.ObjectModel;
using System.Linq;

namespace EmployeeDocumentManagementApp
{
    public class EmployeeRepository
    {
        private static ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

        public static ObservableCollection<Employee> Employees
        {
            get { return employees; }
        }

        public static void AddEmployee(Employee employee)
        {
            employee.EmployeeId = GenerateUniqueId();

            employees.Add(employee);
        }

        public static Employee GetEmployeeByName(string name)
        {
            return employees.FirstOrDefault(e => e.EmployeeName == name);
        }

        private static int GenerateUniqueId()
        {
            return employees.Count + 1;
        }
        public static void UpdateEmployee(Employee employee)
        {
            int index = employees.FindIndex(e => e.EmployeeId == employee.EmployeeId);

            if (index != -1)
            {
                employees[index] = employee;
            }
        }

    }
}
