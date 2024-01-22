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
            employees.Add(employee);
        }

        public static Employee GetEmployeeByName(string name)
        {
            return employees.FirstOrDefault(e => e.EmployeeName == name);
        }
    }
}
