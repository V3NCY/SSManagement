using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public class EmployeeRepository
    {
        internal static ObservableCollection<Employee> employeesList = new ObservableCollection<Employee>();
        private static AppDbContext context = new AppDbContext();
        private static Random random = new Random();
        private static ObservableCollection<Employee> archivedEmployees = new ObservableCollection<Employee>();

        public static ObservableCollection<Employee> GetEmployeesList()
        {
            LoadEmployeesFromDatabase();
            return employeesList;
        }

        public static void ArchiveEmployee(Employee employee)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var existingEmployee = context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
                    if (existingEmployee != null)
                    {
                        existingEmployee.IsArchived = true;
                        context.SaveChanges();

                        employeesList.Remove(existingEmployee);

                        archivedEmployees.Add(existingEmployee);

                        MessageBox.Show($"Employee {existingEmployee.EmployeeId} archived successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Employee with ID {employee.EmployeeId} not found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error archiving employee: {ex.Message}");
                MessageBox.Show($"Error archiving employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public static void AddEmployee(Employee employee)
        {
            try
            {
                context.Employees.Add(employee);
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationError in ex.EntityValidationErrors.SelectMany(e => e.ValidationErrors))
                {
                    Console.WriteLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding employee: {ex.Message}");
                MessageBox.Show($"Error adding employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void UpdateEmployee(Employee employee)
        {
            try
            {
                var existingEmployee = context.Employees.Find(employee.EmployeeId);
                if (existingEmployee != null)
                {
                    context.Entry(existingEmployee).CurrentValues.SetValues(employee);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating employee: {ex.Message}");
                throw;
            }
        }

        public static void LoadEmployeesFromDatabase()
        {
            employeesList = new ObservableCollection<Employee>(context.Employees.ToList());
        }

        public static void SaveArchivedEmployees() 
        {
            try
            {
                using (var fileStream = File.Create("archivedEmployees.dat"))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, archivedEmployees);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving archived employees: {ex.Message}");
                throw;
            }
        }
        public static Employee GetEmployeeByName(string name)
        {
            try
            {
                return context.Employees.FirstOrDefault(e => e.EmployeeName == name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting employee by name: {ex.Message}");
                throw;
            }
        }

        private static void LogErrorDetails(Exception ex)
        {
            Console.WriteLine($"Error Type: {ex.GetType().FullName}");
            Console.WriteLine($"Error Message: {ex.Message}");
            Console.WriteLine($"Error.StackTrace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                Console.WriteLine($"InnerException Type: {ex.InnerException.GetType().FullName}");
                Console.WriteLine($"InnerException Message: {ex.InnerException.Message}");
                Console.WriteLine($"InnerException.StackTrace: {ex.InnerException.StackTrace}");
            }
        }

        private static int GenerateUniqueId()
        {
            return random.Next(100, 1000);
        }
    }
}
