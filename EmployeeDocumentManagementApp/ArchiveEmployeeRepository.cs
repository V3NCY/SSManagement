using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace EmployeeDocumentManagementApp
{
    public static class ArchiveEmployeeRepository
    {
        private static ObservableCollection<Employee> archivedEmployees;

        static ArchiveEmployeeRepository()
        {
            LoadArchivedEmployees();
        }

        public static ObservableCollection<Employee> GetArchivedEmployees()
        {
            return archivedEmployees;
        }

        public static void ArchiveEmployee(Employee employee)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    // Ensure the employee exists in the database
                    var existingEmployee = context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
                    if (existingEmployee != null)
                    {
                        // Update the IsArchived property
                        existingEmployee.IsArchived = true;

                        // Perform data validation before saving changes
                        var validationContext = new ValidationContext(existingEmployee, serviceProvider: null, items: null);
                        var validationResults = new List<ValidationResult>();
                        if (!Validator.TryValidateObject(existingEmployee, validationContext, validationResults, validateAllProperties: true))
                        {
                            // Print validation errors
                            foreach (var validationResult in validationResults)
                            {
                                Console.WriteLine($"Validation Error: {validationResult.ErrorMessage}");
                            }
                            return;
                        }

                        // Save changes to the database
                        context.SaveChanges();

                        // Log successful archiving
                        Console.WriteLine($"Employee {existingEmployee.EmployeeId} archived successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Employee with ID {employee.EmployeeId} not found in the database.");
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Log validation errors
                foreach (var validationError in ex.EntityValidationErrors.SelectMany(validationResult => validationResult.ValidationErrors))
                {
                    Console.WriteLine($"Validation Error: {validationError.ErrorMessage}");
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                // Log database update error
                Console.WriteLine($"Error updating the database: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Log other exceptions
                Console.WriteLine($"Error archiving employee: {ex.Message}");
                throw;
            }
        }


        public static void LoadArchivedEmployees()
        {
            if (File.Exists("archivedEmployees.dat"))
            {
                using (var fileStream = File.OpenRead("archivedEmployees.dat"))
                {
                    if (fileStream.Length > 0)
                    {
                        var formatter = new BinaryFormatter();
                        archivedEmployees = (ObservableCollection<Employee>)formatter.Deserialize(fileStream);
                    }
                    else
                    {
                        archivedEmployees = new ObservableCollection<Employee>();
                    }
                }
            }
            else
            {
                archivedEmployees = new ObservableCollection<Employee>();
            }
        }

        public static void SaveArchivedEmployees()
        {
            using (var fileStream = File.Create("archivedEmployees.dat"))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, archivedEmployees);
            }
        }
    }
}