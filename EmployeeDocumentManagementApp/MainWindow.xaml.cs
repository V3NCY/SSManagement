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
            leaveRequestsWindow.ShowDialog();
        }


        private void OnSickLeaveButtonClick(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new SickLeaveWindow());
        }

    }
}
