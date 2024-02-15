using System;
using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeRegistrationWindow : Window
    {
        private readonly EmployeeListWindow _employeeListWindow;

        public EmployeeRegistrationWindow(EmployeeListWindow employeeListWindow)
        {
            InitializeComponent();
            _employeeListWindow = employeeListWindow;
        }

        private void OnRegisterButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                string jobTitle = txtJobTitle.Text;
                string department = txtDepartment.Text;
                int remainingLeaveDays = 20;

                var newEmployee = new Employee
                {
                    FirstName = firstName,
                    LastName = lastName,
                    JobTitle = jobTitle,
                    Department = department,
                    RemainingLeaveDays = remainingLeaveDays,
                };

                EmployeeRepository.AddEmployee(newEmployee);
                _employeeListWindow.LoadEmployeeList(); 

                MessageBox.Show($"Employee added successfully! Name: {newEmployee.FirstName} {newEmployee.LastName}, ID: {newEmployee.EmployeeId}");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding employee: {ex.Message}");
            }
        }

    }
}
