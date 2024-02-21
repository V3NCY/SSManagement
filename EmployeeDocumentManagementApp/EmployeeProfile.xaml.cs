using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeProfile : Window
    {
        public EmployeeProfile(Employee employee)
        {
            InitializeComponent();
            DataContext = employee;
        }
        private void OnEmployeeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (employeeListBox.SelectedItem != null)
            {
                Employee selectedEmployee = (Employee)employeeListBox.SelectedItem;
                List<Employee> selectedEmployees = new List<Employee> { selectedEmployee };
                EmployeeDetailsWindow detailsWindow = new EmployeeDetailsWindow(selectedEmployees);
                detailsWindow.Show();
            }
        }
    }
}
