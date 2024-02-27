using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
        private void OnEmployeeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Employee selectedEmployee = (Employee)employeeListView.SelectedItem;

            if (selectedEmployee != null)
            {
                bool isWindowOpen = false;
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(EmployeeProfile))
                    {
                        EmployeeProfile profileWindow = (EmployeeProfile)window;
                        if (profileWindow.DataContext == selectedEmployee)
                        {
                            isWindowOpen = true;
                            profileWindow.Activate();
                            break;
                        }
                    }
                }

                if (!isWindowOpen)
                {
                    EmployeeProfile profileWindow = new EmployeeProfile(selectedEmployee);
                    profileWindow.Title = "Лично досие - " + selectedEmployee.FullName;
                    profileWindow.Show();
                }
            }
        }
    }
}
