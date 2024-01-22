using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            EmployeeRegistrationWindow registrationWindow = new EmployeeRegistrationWindow();
            registrationWindow.Show();

            //registrationWindow.Closed += (s, args) => EmployeeListWindow.RefreshEmployeeList();
        }

        private void OnViewEmployeeListButtonClick(object sender, RoutedEventArgs e)
        {
            EmployeeListWindow employeeListWindow = new EmployeeListWindow();
            employeeListWindow.Show();
        }

    }
}
