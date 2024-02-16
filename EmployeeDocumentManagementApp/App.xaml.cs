using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Load employees and archived employees on application startup
            EmployeeRepository.GetEmployeesList();
            ArchiveEmployeeRepository.LoadArchivedEmployees();

            // Test database connection
            //TestDatabaseConnection();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Save changes to archived employees when exiting the application
            ArchiveEmployeeRepository.SaveArchivedEmployees();
        }

        //private void TestDatabaseConnection()
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["DBEmployees"].ConnectionString;

        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            MessageBox.Show("Database connection successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Error connecting to the database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //}
    }
}
