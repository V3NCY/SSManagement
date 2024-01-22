using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class SpecialRequestsWindow : Window
    {
        public SpecialRequestsWindow()
        {
            InitializeComponent();
        }

        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Special request submitted successfully!");

            Close();
        }
    }
}
