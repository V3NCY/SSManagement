using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

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

        public static void LoadArchivedEmployees()
        {
            string filePath = "archivedEmployees.dat";

            if (File.Exists(filePath))
            {
                using (var fileStream = File.OpenRead(filePath))
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
                Console.WriteLine($"Проблем при запазване на архивираните служители: {ex.Message}");
                throw;
            }
        }

        public static void ArchiveEmployee(Employee employee)
        {
            try
            {
                var existingEmployee = EmployeeRepository.GetEmployeesList().FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
                if (existingEmployee != null)
                {
                    existingEmployee.IsArchived = true;
                    EmployeeRepository.SaveArchivedEmployees();
                    EmployeeRepository.GetEmployeesList().Remove(existingEmployee);
                    archivedEmployees.Add(existingEmployee);
                    SaveArchivedEmployees();
                    MessageBox.Show($"Служителят {existingEmployee.EmployeeId} е архивиран успешно.", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Служителят с ID {employee.EmployeeId} не беше намерен в базата с данни.", "Грешка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Проблем при архивиране на служителя: {ex.Message}");
                MessageBox.Show($"Проблем при архивиране на служителя: {ex.Message}", "Грешка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AddArchivedEmployee(Employee employee)
        {
            archivedEmployees.Add(employee);
            SaveArchivedEmployees();
        }
    }
}
