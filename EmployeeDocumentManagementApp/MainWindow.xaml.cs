using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;


namespace EmployeeDocumentManagementApp
{
    public partial class MainWindow : Window
    {

        private EmployeeListWindow employeeListWindow;
        public MainWindow()
        {
            InitializeComponent();
            employeeListWindow = new EmployeeListWindow();
        }

        private void OnLeaveRequestsButtonClick(object sender, RoutedEventArgs e)
        {
            LeaveRequestsWindow leaveRequestsWindow = new LeaveRequestsWindow();
            leaveRequestsWindow.Show();
        }

        private void OnSickLeaveButtonClick(object sender, RoutedEventArgs e)
        {
            SickLeaveWindow sickLeaveWindow = new SickLeaveWindow();
            sickLeaveWindow.Show();
        }

        private void OnSpecialRequestsButtonClick(object sender, RoutedEventArgs e)
        {
            SpecialRequestsWindow specialRequestsWindow = new SpecialRequestsWindow();
            specialRequestsWindow.Show();
        }

        private void OnEmployeeRegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            if (employeeListWindow != null)
            {
                EmployeeRegistrationWindow registrationWindow = new EmployeeRegistrationWindow(employeeListWindow);

                if (registrationWindow.IsInitialized)
                {
                    registrationWindow.Closing += EmployeeRegistrationWindow_Closing;
                    registrationWindow.Show();
                }
            }
        }

        private void EmployeeRegistrationWindow_Closing(object sender, EventArgs e)
        {
            if (employeeListWindow != null)
            {
                employeeListWindow.LoadEmployeeList();
            }
        }


        private void OnArchiveListButtonClick(object sender, RoutedEventArgs e)
        {
            ArchiveListWindow archiveListWindow = new ArchiveListWindow();
            archiveListWindow.Show();
        }

        private void OnViewEmployeeListButtonClick(object sender, RoutedEventArgs e)
        {
            if (employeeListWindow == null || !employeeListWindow.IsLoaded)
            {
                employeeListWindow = new EmployeeListWindow();
                employeeListWindow.Closed += (s, args) => employeeListWindow = null;
            }

            employeeListWindow.Show();
        }


        private void OnFileButtonClick(object sender, RoutedEventArgs e)
        {
            List<Employee> employees;

            using (var dbContext = new AppDbContext())
            {
                employees = dbContext.Employees.ToList();
            }

            if (employees != null && employees.Any())
            {
                EmployeeDetailsWindow employeeDetailsWindow = new EmployeeDetailsWindow(employees);
                employeeDetailsWindow.Show();
            }
            else
            {
                MessageBox.Show("No employees available.");
            }
        }


    }
}