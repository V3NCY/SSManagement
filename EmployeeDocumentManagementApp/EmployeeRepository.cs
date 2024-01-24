using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            try
            {
                employee.EmployeeId = GenerateUniqueId();
                context.Employees.Add(employee);
                context.SaveChanges();
                employeesList.Add(employee);
                refreshCallback?.Invoke();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationError in ex.EntityValidationErrors.SelectMany(e => e.ValidationErrors))
                {
                    Console.WriteLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                }

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
