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
                int.TryParse(txtEGN.Text, out int EGN);
                string department = txtDepartment.Text;
                int remainingLeaveDays = 20;

                var newEmployee = new Employee
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EGN = EGN,
                    JobTitle = jobTitle,
                    Department = department,
                    RemainingLeaveDays = remainingLeaveDays,
                };

                EmployeeRepository.AddEmployee(newEmployee);
                _employeeListWindow.LoadEmployeeList(); 

                MessageBox.Show($"Служителят е добавен успешно! Име: {newEmployee.FirstName} {newEmployee.LastName}, ID: {newEmployee.EmployeeId}");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Грешка при добавяне на служителя: {ex.Message}");
            }
        }

    }
}
