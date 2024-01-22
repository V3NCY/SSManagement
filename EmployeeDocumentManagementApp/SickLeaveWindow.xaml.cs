using System.Windows;

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
            MessageBox.Show("Leave request submitted successfully!");

            Close();
        }
    }
}
