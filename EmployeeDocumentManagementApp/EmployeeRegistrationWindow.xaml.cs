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
            _onEmployeeAdded?.Invoke();
            EmployeeRepository.AddEmployee(newEmployee);

            MessageBox.Show("Служителят е добавен успешно!");
        }
    }
}