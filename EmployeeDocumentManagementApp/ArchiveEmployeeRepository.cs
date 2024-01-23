using System.Collections.Generic;

namespace EmployeeDocumentManagementApp
{
    public static class ArchiveEmployeeRepository
    {
        private static List<Employee> archivedEmployees = new List<Employee>();

        public static void AddToArchive(Employee employee)
        {
            archivedEmployees.Add(employee);
        }

    }
}
