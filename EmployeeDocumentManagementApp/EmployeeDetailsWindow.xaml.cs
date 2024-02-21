using System.Collections.ObjectModel;
using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeDetailsWindow : Window
    {
        public ObservableCollection<Employee> Employees { get; set; }

        public EmployeeDetailsWindow(ObservableCollection<Employee> employees)
        {
            InitializeComponent();
            Employees = employees;
            employeeListView.ItemsSource = Employees;
        }
    }
}
