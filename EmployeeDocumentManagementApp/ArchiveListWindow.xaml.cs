using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class ArchiveListWindow : Window
    {
        private static ArchiveListWindow archiveListWindow;

        public ArchiveListWindow()
        {
            InitializeComponent();
            LoadArchivedEmployees();
        }

        private void LoadArchivedEmployees()
        {
            lvArchivedEmployees.ItemsSource = ArchiveEmployeeRepository.GetArchivedEmployees();
        }

        private void OnArchiveListButtonClick(object sender, RoutedEventArgs e)
        {
            if (archiveListWindow == null || !archiveListWindow.IsVisible)
            {
                archiveListWindow = new ArchiveListWindow();
                archiveListWindow.Show();
            }
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            LoadArchivedEmployees();
        }
    }
}
