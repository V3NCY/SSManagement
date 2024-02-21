using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeProfile : Window
    {
        public EmployeeProfile(Employee employee)
        {
            InitializeComponent();
            DataContext = employee;
        }
        


    }
}
