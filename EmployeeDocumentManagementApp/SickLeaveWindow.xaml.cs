using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EmployeeDocumentManagementApp
{
    public partial class SickLeaveWindow : Page
    {
        public SickLeaveWindow()
        {
            InitializeComponent();
        }

        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            // Your submission logic here
            MessageBox.Show("Sick leave submitted successfully!");
            NavigationService?.GoBack();
        }
    }
}
