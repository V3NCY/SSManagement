using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeDetailsWindow : Window
    {
        public List<Employee> Employees { get; set; }
        public ObservableCollection<Employee> Employee { get; set; }

        public EmployeeDetailsWindow(List<Employee> employees)
        {
            InitializeComponent();
            Employees = employees;
            employeeListView.ItemsSource = Employees;
        }
        private void OnEmployeeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Get the selected employee from the ListView
            Employee selectedEmployee = (Employee)employeeListView.SelectedItem;

            if (selectedEmployee != null)
            {
                // Create a new instance of the EmployeeProfile window with the selected employee
                EmployeeProfile profileWindow = new EmployeeProfile(selectedEmployee);

                // Show the EmployeeProfile window
                profileWindow.Show();
            }
        }
    }
}
