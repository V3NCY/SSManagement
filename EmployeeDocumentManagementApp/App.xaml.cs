using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Windows;
using OfficeOpenXml.Packaging.Ionic.Zip;

namespace EmployeeDocumentManagementApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ArchiveEmployeeRepository.LoadArchivedEmployees();
            TestDatabaseConnection();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ArchiveEmployeeRepository.SaveArchivedEmployees();

            base.OnExit(e);
        }
        private void TestDatabaseConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBEmployees"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Database connection successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error connecting to the database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}