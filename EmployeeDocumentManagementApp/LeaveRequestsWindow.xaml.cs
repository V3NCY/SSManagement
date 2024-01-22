using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EmployeeDocumentManagementApp
{
    public partial class LeaveRequestsWindow : Window
    {
        public LeaveRequestsWindow()
        {
            InitializeComponent();
        }

        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Leave request submitted successfully!");

            // Get the NavigationService from the current page
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            navigationService?.GoBack();
        }


    }
}
