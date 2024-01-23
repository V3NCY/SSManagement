using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ArchiveEmployeeRepository.LoadArchivedEmployees();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ArchiveEmployeeRepository.SaveArchivedEmployees();

            base.OnExit(e);
        }
    }
}
