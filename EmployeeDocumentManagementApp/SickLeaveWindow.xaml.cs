using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EmployeeDocumentManagementApp
{
    public partial class SickLeaveWindow : Window
    {
        public SickLeaveWindow()
        {
            InitializeComponent();
        }

        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sick leave submitted successfully!");

            // Cast sender to SickLeaveWindow
            SickLeaveWindow sickLeaveWindow = sender as SickLeaveWindow;

            // Get the NavigationService from the current page
            NavigationService navigationService = NavigationService.GetNavigationService(sickLeaveWindow);
            navigationService?.GoBack();
        }


    }
}
