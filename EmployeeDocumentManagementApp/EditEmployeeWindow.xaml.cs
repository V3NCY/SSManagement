using System;
using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class EditEmployeeWindow : Window
    {
        private readonly EmployeeListWindow _employeeListWindow;
        private readonly Employee _selectedEmployee;

        public EditEmployeeWindow(EmployeeListWindow employeeListWindow, Employee selectedEmployee)
        {
            InitializeComponent();
            _employeeListWindow = employeeListWindow;
            _selectedEmployee = selectedEmployee;
            DataContext = selectedEmployee; 
        }

        private void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _selectedEmployee.FirstName = txtFirstName.Text;
                _selectedEmployee.LastName = txtLastName.Text;
                _selectedEmployee.JobTitle = txtJobTitle.Text;
                _selectedEmployee.Department = txtDepartment.Text;

                EmployeeRepository.UpdateEmployee(_selectedEmployee);

                _employeeListWindow.LoadEmployeeList();

                MessageBox.Show($"Служителят е редактиран успешно! Име: {_selectedEmployee.FirstName} {_selectedEmployee.LastName}, ID: {_selectedEmployee.EmployeeId}");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Проблем при редактиране на служителя: {ex.Message}");
            }
        }
    }
}
