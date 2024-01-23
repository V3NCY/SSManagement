using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class ArchiveListWindow : Window
    {
        public ArchiveListWindow()
        {
            InitializeComponent();
            LoadArchivedEmployees();
        }

        private void LoadArchivedEmployees()
        {
            lvArchivedEmployees.ItemsSource = ArchiveEmployeeRepository.GetArchivedEmployees();
        }
    }
}
