using System.Windows;

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
                EmployeeRegistrationWindow registrationWindow = new EmployeeRegistrationWindow(() => employeeListWindow.LoadEmployeeList());

                if (registrationWindow.IsInitialized)
                {
                    registrationWindow.Closing += (s, args) => employeeListWindow.LoadEmployeeList();
                    registrationWindow.Show();
                }
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
    }
}