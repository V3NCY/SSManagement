using System;
using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeRegistrationWindow : Window
    {
        private readonly Action _onEmployeeAdded;

        public EmployeeRegistrationWindow(Action onEmployeeAdded)
        {
            InitializeComponent();
            _onEmployeeAdded = onEmployeeAdded;
        }

        private void OnRegisterButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string employeeName = txtEmployeeName.Text;
                string jobTitle = txtJobTitle.Text;
                string department = txtDepartment.Text;

                int remainingLeaveDays = 20;

                Employee newEmployee = new Employee
                {
                    EmployeeName = employeeName,
                    JobTitle = jobTitle,
                    Department = department,
                    RemainingLeaveDays = remainingLeaveDays,
                };

                EmployeeRepository.AddEmployee(newEmployee, _onEmployeeAdded);
                _onEmployeeAdded?.Invoke(); // Refresh the list immediately

                MessageBox.Show($"Employee added successfully: {newEmployee.EmployeeName}, ID: {newEmployee.EmployeeId}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding employee: {ex.Message}");
            }
        }


    }
}