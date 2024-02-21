using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeDetailsWindow : Window
    {
        public List<Employee> Employees { get; set; }
        public ObservableCollection<Employee> Employee { get; set; }

        public EmployeeDetailsWindow(List<Employee> employees)
        {
            InitializeComponent();
            Employees = employees;
            employeeListView.ItemsSource = Employees;
        }
    }
}
