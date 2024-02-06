using System;
using System.Runtime.Remoting.Contexts;
using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeRegistrationWindow : Window
    {
        private readonly Action _onEmployeeAdded;
        private static AppDbContext context = new AppDbContext();
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
                MessageBox.Show($"Служителят е добавен успешно! Име: {newEmployee.EmployeeName}, ID: {newEmployee.EmployeeId}");
                context.SaveChanges();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding employee: {ex.Message}");
                _onEmployeeAdded?.Invoke(); 
            }
        }

    }
}
