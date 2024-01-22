using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeListWindow : Window
    {
        private static EmployeeListWindow instance;

        public EmployeeListWindow()
        {
            InitializeComponent();
            LoadEmployeeList();
            SubscribeToEmployeeChanges();
        }

        private void LoadEmployeeList()
        {
            lvEmployees.ItemsSource = EmployeeRepository.Employees;
        }

        private void SubscribeToEmployeeChanges()
        {
            EmployeeRepository.Employees.CollectionChanged += (sender, e) => LoadEmployeeList();
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            LoadEmployeeList();
        }
    }
}
