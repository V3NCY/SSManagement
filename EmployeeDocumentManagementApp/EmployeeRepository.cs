using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO; 
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace EmployeeDocumentManagementApp
{
    public class EmployeeRepository
    {
        private static AppDbContext context = new AppDbContext();
        private static Random random = new Random();
        private static ObservableCollection<Employee> employeesList = new ObservableCollection<Employee>();
        private static ObservableCollection<Employee> archivedEmployees = new ObservableCollection<Employee>();

        public static ObservableCollection<Employee> GetEmployeesList()
        {
            return employeesList;
        }

        public static void AddEmployee(Employee employee, Action refreshCallback = null)
        {
            try
            {
                employee.EmployeeId = GenerateUniqueId();
                context.Employees.Add(employee);
                employeesList.Add(employee);
                context.SaveChanges();

                refreshCallback?.Invoke();
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
                LogErrorDetails(ex);
                throw;
            }
        }
        private void OnClosing(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream("employees.bin", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, employeesList);
            }
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            if (File.Exists("employees.bin"))
            {
                using (FileStream fs = new FileStream("employees.bin", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    employeesList = (ObservableCollection<Employee>)formatter.Deserialize(fs);
                }
            }
        }
        public static void ArchiveEmployee(Employee employee)
        {
            try
            {
                employee.IsArchived = true;
                employeesList.Remove(employee);
                archivedEmployees.Add(employee);
                SaveArchivedEmployees();
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error archiving employee: {ex.Message}");
                throw;
            }
        }


        private static void SaveArchivedEmployees()
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

        private static int GenerateUniqueId()
        {
            return random.Next(100, 1000);
        }
    }
}
