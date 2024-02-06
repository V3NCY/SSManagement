using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;

namespace EmployeeDocumentManagementApp
{
    public class EmployeeRepository
    {
        private static AppDbContext context = new AppDbContext();
        private static Random random = new Random();

        private static ObservableCollection<Employee> employeesList = new ObservableCollection<Employee>();
        public static ObservableCollection<Employee> GetEmployeesList()
        {
            return employeesList;
        }

        public static void AddEmployee(Employee employee, Action refreshCallback = null)
        {
                employee.EmployeeId = GenerateUniqueId();
            try
            {
                context.Employees.Add(employee);

                employeesList.Add(employee);
                Console.WriteLine("Before SaveChanges");
                context.SaveChanges();
                Console.WriteLine("After SaveChanges");


                refreshCallback?.Invoke();

                Console.WriteLine("Current employees in employeesList:");
                foreach (var emp in employeesList)
                {
                    Console.WriteLine($"{emp.EmployeeName}, ID: {emp.EmployeeId}");
                }
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


        private static void ValidateEmployee(Employee employee)
        {
            var validationContext = new ValidationContext(employee, null, null);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(employee, validationContext, validationResults, validateAllProperties: true))
            {
                var validationErrors = validationResults.Select(result => $"{result.MemberNames.FirstOrDefault()}: {result.ErrorMessage}");
                throw new DbEntityValidationException($"Validation failed for Employee. Errors: {string.Join(", ", validationErrors)}");
            }
        }
        private static void LogErrorDetails(Exception ex)
        {
            Console.WriteLine($"Error Type: {ex.GetType().FullName}");
            Console.WriteLine($"Error Message: {ex.Message}");
            Console.WriteLine($"Error.StackTrace: {ex.StackTrace}");

            if (ex is DbEntityValidationException validationException)
            {
                foreach (var entityValidationError in validationException.EntityValidationErrors.SelectMany(e => e.ValidationErrors))
                {
                    Console.WriteLine($"Entity Validation Error - Property: {entityValidationError.PropertyName}, Error: {entityValidationError.ErrorMessage}");
                }
            }

            if (ex.InnerException != null)
            {
                Console.WriteLine($"InnerException Type: {ex.InnerException.GetType().FullName}");
                Console.WriteLine($"InnerException Message: {ex.InnerException.Message}");
                Console.WriteLine($"InnerException.StackTrace: {ex.InnerException.StackTrace}");
            }
        }
        public static void ArchiveEmployee(Employee employee)
        {
            try
            {
                employee.IsArchived = true;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error archiving employee: {ex.Message}");
                throw;
            }
        }

        /*
        public static void RemoveEmployee(Employee employee)
        {
            try
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing employee: {ex.Message}");
                throw; // Rethrow the exception for upper layers to handle
            }
        }
        */

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

        public static void UpdateEmployee(Employee employee)
        {
            try
            {
                var existingEmployee = context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
                if (existingEmployee != null)
                {
                    existingEmployee.FirstName = employee.FirstName;
                    existingEmployee.LastName = employee.LastName;
                    existingEmployee.EGN = employee.EGN;
                    existingEmployee.EmployeeName = employee.EmployeeName;
                    existingEmployee.RemainingLeaveDays = employee.RemainingLeaveDays;
                    existingEmployee.JobTitle = employee.JobTitle;
                    existingEmployee.Department = employee.Department;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating employee: {ex.Message}");
                throw;
            }
        }

        public static ObservableCollection<Employee> GetArchivedEmployees()
        {
            try
            {
                var archivedEmployeesList = context.Employees.Where(e => e.IsArchived).ToList();
                return new ObservableCollection<Employee>(archivedEmployeesList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting archived employees: {ex.Message}");
                throw;
            }
        }
    }
}