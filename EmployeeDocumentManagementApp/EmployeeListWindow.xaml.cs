using System.Collections.ObjectModel;
using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeListWindow : Window
    {
        private static readonly AppDbContext context = new AppDbContext();

        public EmployeeListWindow()
        {
            InitializeComponent();
            LoadEmployeeList();
            SubscribeToEmployeeChanges();
        }

        private void LoadEmployeeList()
        {
            lvEmployees.ItemsSource = EmployeeRepository.GetEmployeesList();
        }

        private void SubscribeToEmployeeChanges()
        {
            var employeesList = EmployeeRepository.GetEmployeesList();

            if (employeesList != null)
            {
                employeesList.CollectionChanged += (sender, e) => LoadEmployeeList();
            }
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            LoadEmployeeList();
        }

        private void OnDeleteMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (lvEmployees.SelectedItem is Employee selectedEmployee && selectedEmployee != null)
            {
                MoveToArchive(selectedEmployee);
            }
        }

        private void MoveToArchive(Employee employee)
        {
            ArchiveEmployeeRepository.AddToArchive(employee);
            RemoveEmployee(employee);
        }

        private void RemoveEmployee(Employee employee)
        {
            context.Employees.Remove(employee);
            context.SaveChanges();
        }
    }
}
