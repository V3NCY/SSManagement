using System;
using System.Collections.Generic;
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
        private static Dictionary<int, List<DateTime>> paidLeaveRecords = new Dictionary<int, List<DateTime>>();

        public static List<DateTime> GetPaidLeaveRecords(int employeeId)
        {
            if (paidLeaveRecords.ContainsKey(employeeId))
            {
                return paidLeaveRecords[employeeId];
            }
            else
            {
                return new List<DateTime>();
            }
        }

        public static void UpdatePaidLeaveRecords(int employeeId, DateTime paidLeaveDate)
        {
            if (paidLeaveRecords.ContainsKey(employeeId))
            {
                paidLeaveRecords[employeeId].Add(paidLeaveDate);
            }
            else
            {
                paidLeaveRecords[employeeId] = new List<DateTime> { paidLeaveDate };
            }
        }
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

                        MessageBox.Show($"Служителят {existingEmployee.EmployeeId} е архивиран успешно!.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Служител с ID {employee.EmployeeId} не бше от крит в базата с данни.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Проблем при архивиране на служителя: {ex.Message}");
                MessageBox.Show($"Проблем при архивиране на служителя: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                Console.WriteLine($"Проблем при добавяне на служителя: {ex.Message}");
                MessageBox.Show($"Проблем при добавяне на служителя: {ex.Message}", "Грешка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                Console.WriteLine($"Проблем с обновяване на служителя: {ex.Message}");
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
                Console.WriteLine($"Грешка при архивиране на служителите: {ex.Message}");
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
                Console.WriteLine($"Проблем при извличане на името на служителя: {ex.Message}");
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
        public static Employee GetEmployeeById(int employeeId)
        {
            try
            {
                return context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving employee by ID: {ex.Message}");
                throw;
            }
        }

        private static int GenerateUniqueId()
        {
            return random.Next(100, 1000);
        }
    }
}
