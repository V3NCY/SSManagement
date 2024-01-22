using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeListWindow : Window
    {
        public EmployeeListWindow()
        {
            InitializeComponent();
            LoadEmployeeList();
        }

        private void LoadEmployeeList()
        {
            // Get the list of employees from the repository
            lvEmployees.ItemsSource = EmployeeRepository.GetEmployees();
        }
    }
}
