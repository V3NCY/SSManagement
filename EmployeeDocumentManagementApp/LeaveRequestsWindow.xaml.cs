using System.Windows;

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
            // Your submission logic here
            MessageBox.Show("Leave request submitted successfully!");

            // You can close the window after submission
            Close();
        }
    }
}
