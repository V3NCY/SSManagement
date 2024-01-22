using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeRegistrationWindow : Window
    {
        public EmployeeRegistrationWindow()
        {
            InitializeComponent();
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

            EmployeeRepository.AddEmployee(newEmployee);

            MessageBox.Show("Служителят е добавен успешно!");
        }
    }
}
