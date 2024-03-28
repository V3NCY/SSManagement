using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;


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
            ToggleButton.IsChecked = false;
            DocumentsPopup.IsOpen = false;
        }

        private void OnSickLeaveButtonClick(object sender, RoutedEventArgs e)
        {
            SickLeaveWindow sickLeaveWindow = new SickLeaveWindow();
            sickLeaveWindow.Show();
            ToggleButton.IsChecked = false;
            DocumentsPopup.IsOpen = false;
        }

        private void OnSpecialRequestsButtonClick(object sender, RoutedEventArgs e)
        {
            SpecialRequestsWindow specialRequestsWindow = new SpecialRequestsWindow();
            specialRequestsWindow.Show();
            ToggleButton.IsChecked = false;
            DocumentsPopup.IsOpen = false;
        }

        private void OnEmployeeRegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            EmployeeRegistrationWindow registrationWindow = new EmployeeRegistrationWindow(employeeListWindow);

            if (registrationWindow.IsInitialized)
            {
                registrationWindow.Closing += EmployeeRegistrationWindow_Closing;
            }
            registrationWindow.Show();

            ToggleButton.IsChecked = false;
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
            ToggleButton.IsChecked = false;
        }
        private void OnTrudovDogovorButtonClick(object sender, RoutedEventArgs e)
        {
            TrudovDogovorWindow trudovDogovorWindow = new TrudovDogovorWindow();
            trudovDogovorWindow.Show();
            DogovoriToggleButton.IsChecked = false;
        }



        private void OnGrazhdanskiDogovorButtonClick(object sender, RoutedEventArgs e)
        {
            GrazhdanskiDogovoriWindow grazhdanskiDogovoriWindow = new GrazhdanskiDogovoriWindow();
            grazhdanskiDogovoriWindow.Show();
            DogovoriToggleButton.IsChecked = false;
        }

        private void OnDogovorAddButtonClick(object sender, RoutedEventArgs e)
        {
            DogovorAddWindow documentsWindow = new DogovorAddWindow();
            documentsWindow.Show();
            DogovoriToggleButton.IsChecked = false;
        }
        private void OnViewEmployeeListButtonClick(object sender, RoutedEventArgs e)
        {
            if (employeeListWindow == null || !employeeListWindow.IsLoaded)
            {
                employeeListWindow = new EmployeeListWindow();
                employeeListWindow.Closed += (s, args) => employeeListWindow = null;
            }

            employeeListWindow.Show();
            ToggleButton.IsChecked = false;
        }
        //private void Popup_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    Popup.IsOpen = false;
        //}
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
            ToggleButton.IsChecked = false;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}