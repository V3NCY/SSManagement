using System.Collections.ObjectModel;
using System.IO;
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
            employee.IsArchived = true;
            archivedEmployees.Add(employee);
            SaveArchivedEmployees();
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
