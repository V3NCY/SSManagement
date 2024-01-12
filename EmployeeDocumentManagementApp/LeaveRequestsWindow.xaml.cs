using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EmployeeDocumentManagementApp
{
    public partial class LeaveRequestsWindow : Page
    {
        public LeaveRequestsWindow()
        {
            InitializeComponent();
        }

        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            // Your submission logic here
            MessageBox.Show("Leave request submitted successfully!");
            NavigationService?.GoBack();
        }
    }
}
